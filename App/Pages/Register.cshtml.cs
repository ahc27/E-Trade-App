using classLib.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace App.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public CreateUserdto newUser { get; set; }
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiGateway");
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            var json = JsonSerializer.Serialize(newUser);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("register", content);

            if (!response.IsSuccessStatusCode)
            {
                var log = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(log);
                return Page();
            }

            return RedirectToPage("/Login");

        }
    }
}
