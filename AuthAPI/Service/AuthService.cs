using classLib;
using classLib.LogDtos;
using System.IdentityModel.Tokens.Jwt;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJWTService _jwtService;
        private readonly RabbitMqProducer _rabbitMqProducer;

        public AuthService(IJWTService jwtService,IHttpClientFactory httpClient, RabbitMqProducer rabbitMqProducer)
        {

            _jwtService = jwtService; 
            _httpClient = httpClient.CreateClient();
            _rabbitMqProducer = rabbitMqProducer;
        }

        public async Task<AuthResponse> Login(UserAuth request)
        {
            var user = await GetUserByEmail(request.email);
            if (user == null) return null;

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.password, user.password);
            if (!passwordMatch) return null;

            user.refreshToken = _jwtService.GenerateRefreshToken(user);

            var updateRequest = new HttpRequestMessage(HttpMethod.Put, "http://user_api:8080/api/Users/refreshtoken")
            {
              Content = JsonContent.Create(user)
            };

            var updateResponse = await _httpClient.SendAsync(updateRequest);

            if (!updateResponse.IsSuccessStatusCode)
            {
                bool failedLog = await LogAuth(null, false, "Login", "User login failed", new ArgumentException("User cannot be found."));
            }

            var token = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                await LogAuth(user.Id.ToString(), false, "Login", "Token generation failed",new InvalidOperationException("Token cannot be  generated") );
                return null;
            }
            
            var authResponse = new AuthResponse { accessToken = token ,refreshToken=user.refreshToken};

            await LogAuth(user.Id.ToString(), true, "Login", "User logged in successfully", null);
            return authResponse;
        }

        public async Task<AuthResponse> Refresh(string refreshToken)
        {
            if (!await _jwtService.isTokenValid(refreshToken))
            {
                await LogAuth(null, false, "Refresh", "Invalid or expired refresh token", null);
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
              var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
              var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

              var email = emailClaim?.Value;

              if (string.IsNullOrEmpty(email))
                  throw new Exception(); 
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                bool failedLog = await LogAuth(null, false, "Login", "User login failed", new ArgumentException("User cannot be found."));
                return null;
            }

            var updateRequest = new HttpRequestMessage(HttpMethod.Put, "http://user_api:8080/api/Users/refreshtoken")
            {
                Content = JsonContent.Create(user)
            };

            var updateResponse = await _httpClient.SendAsync(updateRequest);

            if (!updateResponse.IsSuccessStatusCode)
            {
                bool failedLog = await LogAuth(null, false, "Login", "User login failed", new ArgumentException("Refresh Token cannot be updated."));
            }

            AuthResponse response = new AuthResponse
            {
                accessToken = _jwtService.GenerateToken(user),
                refreshToken = _jwtService.GenerateRefreshToken(user)
            };

            return response;
        }

        public async Task<bool> LogAuth(string? entityId, bool success, string action, string message,Exception? exception)
        {
            try
            {
                
                var userLog = new Log
                {
                    IsSuccess = success,
                    Action = action,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    EntityId = entityId,
                    ServiceName = "AuthAPI",
                    Level = success ? "Information" : "Error",
                    Exception = exception
                };

                await _rabbitMqProducer.SendLogAsync(userLog);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
                return false;
            }
        }


        public async Task<AuthorizationDto> GetUserByEmail(string email)
        {
            //From query ile yapmak daha esnek olacaktır
            var response = await _httpClient.GetAsync($"http://user_api:8080/api/Users/email/{email}");
            if (!response.IsSuccessStatusCode) return null;
            if (response.Content == null) return null;
            var user = await response.Content.ReadFromJsonAsync<AuthorizationDto>();

            return user;
        }



    }
}

