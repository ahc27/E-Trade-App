using APIGateway.Service;
using APIGateway.Service.Dto;
using classLib;
using classLib.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(content);

            if (authResponse == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }
            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string request)
        {

            if (request == null)
                return BadRequest(new { message = "Invalid request" });

            var content = await _gatewayService.RefreshToken(request);

            if (content==null)
            {
                return BadRequest(new { message = "Invalid request" });
            }
            return Ok(content);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody] CreateUserdto createUserdto )
        {
            if (createUserdto == null)
                return BadRequest(new { message = "Invalid request" });

            var isRegistered = await _gatewayService.Register(createUserdto);

            if (!isRegistered)
            {
                return BadRequest();
            }

            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _gatewayService.GetAllUsers(Request);

            if (users == null)
                return NotFound("No users found");

            return Ok(users);
        }

        [HttpGet("UserbyId/{id}")]
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
            return Ok(content);
        }

        [HttpGet("CategoryById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id == null || id < 0) return BadRequest("Id is not valid");

            var content = await _gatewayService.GetCategoryById(id);

            if (content == null) return NotFound("Invalid Id");

            return Ok(content);
        }


    }
}