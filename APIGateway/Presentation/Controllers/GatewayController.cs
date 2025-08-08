using APIGateway.Service;
using APIGateway.Service.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _gatewayService;

        public GatewayController( IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;

        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {

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
            Console.WriteLine("📍 Gateway Controller => GetAllUsers çağrıldı");

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
            // "Request" burada doğrudan geçerli
            var users = await _gatewayService.GetAllUsers(Request);

            if (users == null || !users.Any())
                return NotFound("No users found");

            return Ok(users);
        }

        [HttpGet("UserbyID/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserbyID(int id)
        {
            if (id ==null || id < 0) return BadRequest("Id is not valid");

            var content = await _gatewayService.GetUserById(id);

            if (content == null) return NotFound("Invalid Id");

            return Ok(content);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var content = await _gatewayService.GetAllCategories();
            if (content == null) return NotFound("No categories found");
            return Content(content, "application/json");
        }

        [HttpGet("CategoryById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id == null || id < 0) return BadRequest("Id is not valid");

            var content = await _gatewayService.GetCategoryById(id);

            if (content == null) return NotFound("Invalid Id");

            return Content(content, "application/json");
        }


    }
}