using APIGateway.Service.Dto;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace APIGateway.Service
{
    public class GatewayService : IGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceUrls _serviceUrls;

        public GatewayService(IHttpClientFactory httpClientFactory, IOptions<ServiceUrls> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _serviceUrls = options.Value;
        }

        public async Task<String> GetAllUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_serviceUrls.UserApi.GetAllUsers}");
            
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching users: {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<String> GetUserById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_serviceUrls.UserApi.GetUserById}/{id}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching user by ID: {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<String> RefreshToken(LoginDto request)
        {
            try
            {
                var authApiUrl = _serviceUrls.AuthApi.Refresh;
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(authApiUrl, httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) return responseContent;

                else return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while refreshing the token: {ex.Message}");
                return null;
            }
        }

        public async Task<String> Login(LoginDto request)
        {
            try
            {
                var authApiUrl = _serviceUrls.AuthApi.Login;

                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(authApiUrl, httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) return responseContent;

                else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return null;
            }
        }


    }
}
