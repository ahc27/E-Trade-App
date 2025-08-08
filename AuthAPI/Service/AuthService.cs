using AutoMapper;
using classLib;
using classLib.LogDtos;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;
        private readonly RabbitMqProducer _rabbitMqProducer;

        public AuthService(IJWTService jwtService, IMapper mapper, IHttpClientFactory httpClient, RabbitMqProducer rabbitMqProducer)
        {
            _mapper = mapper;
            _jwtService = jwtService; 
            _httpClient = httpClient.CreateClient();
            _rabbitMqProducer = rabbitMqProducer;
        }

        public async Task<string?> Login(UserAuth request)
        {
            var user = await GetUserByEmail(request.email);
            if (user == null) return null;

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.password, user.password);
            if (!passwordMatch) return null;

            if (!await _jwtService.isTokenValid(user.refreshToken))
            {
                user.refreshToken = _jwtService.GenerateRefreshToken(user);

                var updateRequest = new HttpRequestMessage(HttpMethod.Put, "http://user_api:8080/api/Users/refreshtoken")
                {
                    Content = JsonContent.Create(user)
                };

                var updateResponse = await _httpClient.SendAsync(updateRequest);

                if (!updateResponse.IsSuccessStatusCode) return null;
            }

            var token = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                await LogAuth(user.Id.ToString(), false, "Login", "Token generation failed",new InvalidOperationException("Token cannot be  generated") );
                return null;
            }
            await LogAuth(user.Id.ToString(), true, "Login", "User logged in successfully", null);
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
            var user = await GetUserByEmail(request.email);
            if (user == null) return null;

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

