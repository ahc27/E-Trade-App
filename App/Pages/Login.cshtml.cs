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
        public string Token { get; set; }

        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiGateway");
        }


        //[IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var json = JsonSerializer.Serialize(Login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("login", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                Token = doc.RootElement.GetProperty("token").GetString();

                // Tarayýcýya cookie olarak gönder
                Response.Cookies.Append("access_token", Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,           
                    SameSite = SameSiteMode.Strict, 
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }
            else
            {
                Token = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
            }

            return Page();
        }
    }


}
