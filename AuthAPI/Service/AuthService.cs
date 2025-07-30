using AuthAPI.Service.Dtos;
using System.IdentityModel.Tokens.Jwt;
using UserMicroservice.Data;
using UserMicroservice.Data.Repositories;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IJWTService _jwtService;

        public AuthService(UserRepository userRepository, IJWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService; // Assuming JWTService is implemented correctly

        }

        public async Task<User> getByEmailAsync(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<string?> login(UserAuth request)
        {
            var user = await _userRepository.getByEmail(request.email);

            if (user == null) return null;

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.password, user.password);

            if (!passwordMatch) return null;

            
            if(!await _jwtService.isTokenValid(user.refreshToken))
            {
                user.refreshToken = _jwtService.GenerateRefreshToken(user);
                await _userRepository.Update(user);
            }


            var token = _jwtService.GenerateToken(user);
            return token;
        }

        public async Task<string> Refresh(UserAuth request)
        {
            /*  var tokenHandler = new JwtSecurityTokenHandler();
              var jwtToken = tokenHandler.ReadJwtToken(expiredToken);
              var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

              var email = emailClaim?.Value;

              if (string.IsNullOrEmpty(email))
                  throw new Exception(); */

            var user = await _userRepository.GetByEmail(request.email);
            if (user == null)
                return null;

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.password, user.password);
            if (!passwordMatch)
                return null;

            if (string.IsNullOrEmpty(user.refreshToken))
                return null;

            if (await _jwtService.isTokenValid(user.refreshToken))
            {
                return _jwtService.GenerateToken(user);
            }
            else
            {
                return null;
            }
        }



    }
}
