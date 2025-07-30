using APIGateway.Service;
using APIGateway.Service.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace APIGateway.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IGatewayService _gatewayService;

        public GatewayController(IHttpClientFactory httpClientFactory, IGatewayService gatewayService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _gatewayService = gatewayService;

        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {

            if (request == null || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
                return BadRequest(new { message = "Invalid request" });

            var content = await _gatewayService.Login(request);

            if (string.IsNullOrEmpty(content))
            {
                return BadRequest(new { message = "Invalid request" });
            }
            return Content(content, "application/json");

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] LoginDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
                return BadRequest(new { message = "Invalid request" });

            var content = await _gatewayService.RefreshToken(request);

            if (string.IsNullOrEmpty(content))
            {
                return BadRequest(new { message = "Invalid request" });
            }
            return Content(content, "application/json");


        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {

            var content = await _gatewayService.GetAllUsers();

            if (content == null) return NotFound(new { message = "No users found" });

            return Content(content, "application/json");
        }

        [HttpGet("UserbyID/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserbyID(int id)
        {
            if (id ==null || id < 0) return BadRequest("Id is not valid");

            var content = await _gatewayService.GetUserById(id);

            if (content == null) return NotFound("Invalid Id");

            return Content(content, "application/json");

        }

    }
}