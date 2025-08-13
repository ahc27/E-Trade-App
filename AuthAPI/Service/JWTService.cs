using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using classLib;

namespace AuthAPI.Service
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(AuthorizationDto user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var key = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub ,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.email),
                new Claim(ClaimTypes.Role, user.role),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokendescriptor);

            return tokenhandler.WriteToken(token);
        }

        public string GenerateRefreshToken(AuthorizationDto user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = _configuration.GetSection("JwtConfig")["Key"];
            int Expires = Int32.Parse(_configuration.GetSection("JwtConfig")["RefreshTokenExpireTime"]);

            var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.email),
            new Claim("tokenType", "refresh"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Expires),
                SigningCredentials = new SigningCredentials(
                    securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokendescriptor);

            return tokenhandler.WriteToken(token);

        }

        public async Task<bool> isTokenValid(string token)
        {
            try
            {

                if (string.IsNullOrEmpty(token))
                    return false;

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig")["Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out SecurityToken validatedToken);

                var email = tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    throw new SecurityTokenException("Email claim missing");
                }

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
            catch (Exception)
            {
                return false; 
            }
        }

    }
}