using classLib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace App.Pages
{
    public class UserPageModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<GetUserDto> Users { get; set; } = new();
        public GetUserDto UserById { get; set; }
        public AuthResponse Tokens { get; set; }  = new();
        public List<GetCategoryDto> Categories { get; set; }
        public GetCategoryDto Category { get; set; }

        public UserPageModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiGateway");
        }

        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {

             Request.Cookies.TryGetValue("access_token", out var accessToken);
             Request.Cookies.TryGetValue("refresh_token", out var refreshToken);

            if (string.IsNullOrEmpty(refreshToken))
            {
                context.Result = RedirectToPage("/Login");
                return;
            }

            var json = JsonSerializer.Serialize(refreshToken);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("refresh", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var responseContent = JsonSerializer.Deserialize<AuthResponse>(responseJson);

            if (!response.IsSuccessStatusCode)
            {
                context.Result = RedirectToPage("/Login");
                return;
            }

            if (responseContent==null)
            {
               context.Result = RedirectToPage("/Login");
                return;
            }

            Response.Cookies.Append("access_token", responseContent.accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                Path = "/"
            });
            Response.Cookies.Append("refresh_token", responseContent.refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                Path = "/"
            });

            Tokens.accessToken=responseContent.accessToken;
            Tokens.refreshToken=responseContent.refreshToken;
            await next.Invoke();
        }

        public async Task OnGetAsync()
        {
           var a = await OnGetAllCategoriesAsync();
        }

        public async Task<JsonResult> OnGetAllUsersAsync()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "Users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.accessToken);
            var response = await _httpClient.SendAsync(request);
 

            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadFromJsonAsync<List<GetUserDto>>();
                return new JsonResult(Users);
            }
            else
            {
                return new JsonResult(new { error = $"API call failed: {response.StatusCode}" });
            }
        }

        public async Task<JsonResult> OnGetUserByIdAsync(int id)
        {

             var request = new HttpRequestMessage(HttpMethod.Get, $"UserbyId/{id}");
             request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.accessToken);

             var response = await _httpClient.SendAsync(request);

             if (response.IsSuccessStatusCode)
             {
                var user = await response.Content.ReadFromJsonAsync<GetUserDto>();
                return new JsonResult(user);
             }
             else
             {
                    return new JsonResult(new { error = $"API call failed: {response.StatusCode}" }) { StatusCode = (int)response.StatusCode };
             }
        }

        public async Task<JsonResult> OnGetAllCategoriesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "AllCategories");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Categories = await response.Content.ReadFromJsonAsync<List<GetCategoryDto>>();
                Console.WriteLine(Categories);
                return new JsonResult(Categories);
            }
            else
            {
                return new JsonResult(new { error = $"API call failed: {response.StatusCode}" });
            }
        }

        public async Task<JsonResult> OnGetCategoryByIdAsync(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"CategorybyId/{id}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Category = await response.Content.ReadFromJsonAsync<GetCategoryDto>();
                    return new JsonResult(Category);
                }
                else
                {
                    return new JsonResult(new { error = $"API call failed: {response.StatusCode}" }) { StatusCode = (int)response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToPage("/Login");
        }



    }
}
