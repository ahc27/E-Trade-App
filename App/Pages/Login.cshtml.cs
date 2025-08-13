using classLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;


namespace App.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserAuth Login { get; set; }
        public AuthResponse Tokens { get; set; }

        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiGateway");
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            var json = JsonSerializer.Serialize(Login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("login", content);

            if (!response.IsSuccessStatusCode)
            {
                var log = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(log);
                return Page();
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);

            if (authResponse != null && !string.IsNullOrEmpty(authResponse.accessToken))
            {
                Response.Cookies.Append("access_token", authResponse.accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                    Path = "/"
                });

                if (!string.IsNullOrEmpty(authResponse.refreshToken))
                {
                    Response.Cookies.Append("refresh_token", authResponse.refreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(100),
                        Path = "/"
                    });
                }
            }

            return RedirectToPage("/UserPage");
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            return RedirectToPage("/Register");
        }
    }   
}
