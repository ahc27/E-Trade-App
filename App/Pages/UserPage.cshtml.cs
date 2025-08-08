using classLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace App.Pages
{
    public class UserPageModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<GetUserDto> Users { get; set; } = new List<GetUserDto>();

        [BindProperty]
        public string Token { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public UserPageModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiGateway");
        }

        public async Task OnGetAsync()
        {

            // Cookie'den token'ý çek
            if (Request.Cookies.TryGetValue("access_token", out string cookieToken))
            {
                Token = cookieToken;
            }
            else
            {
                Response.Redirect("/Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Eðer token form'dan geliyorsa onu kullan
                if (string.IsNullOrEmpty(Token))
                {
                    Token = HttpContext.Session.GetString("AuthToken") ?? string.Empty;
                }

                if (string.IsNullOrEmpty(Token))
                {
                    ErrorMessage = "No token provided. Please login first.";
                    return Page();
                }

                Users = await GetUsersAsync(Token);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }

            return Page();
        }

        private async Task<List<GetUserDto>> GetUsersAsync(string token)
        {
            try
            {

                Console.WriteLine("aaa");

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync("Users");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<GetUserDto>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return users ?? new List<GetUserDto>();
                }
                else
                {
                    ErrorMessage = $"API call failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    return new List<GetUserDto>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error calling API: {ex.Message}";
                return new List<GetUserDto>();
            }
        }
    }
}