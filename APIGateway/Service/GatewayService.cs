using APIGateway.Service.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using classLib;
using classLib.UserDtos;

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
        public async Task<string> Login(LoginDto request)
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

        public async Task<AuthResponse> RefreshToken(string request)
        {
            try
            {
                var authApiUrl = _serviceUrls.AuthApi.Refresh;
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(authApiUrl, httpContent);
                var responseJson = await response.Content.ReadAsStringAsync();

                var responseContent = JsonSerializer.Deserialize<AuthResponse>(responseJson);

                if (response.IsSuccessStatusCode) return responseContent;

                else return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while refreshing the token: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Register(CreateUserdto createUserdto)
        {
            if (createUserdto == null) return false;

            var registerUrl = _serviceUrls.UserApi.AddUser;
            var json = JsonSerializer.Serialize(createUserdto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, registerUrl);
            request.Content = content;
            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<GetUserDto>?> GetAllUsers(HttpRequest request)
        {
            var response = await _httpClient.GetAsync(_serviceUrls.UserApi.GetAllUsers);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<GetUserDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
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

        public async Task<String> GetAllCategories()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _serviceUrls.CategoryApi.GetAllCategories);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching all categories: {response.ReasonPhrase}");
            }
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<GetCategoryDto> GetCategoryById(int id)
        {
            var url = _serviceUrls.CategoryApi.GetCategoryById.Replace("{id}", id.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching category by ID: {response.ReasonPhrase}");
            }
            return await response.Content.ReadFromJsonAsync<GetCategoryDto>();
        }



        }
}
