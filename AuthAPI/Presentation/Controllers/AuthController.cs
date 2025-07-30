using AuthAPI.Service;
using AuthAPI.Service.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

   
    public AuthController(IAuthService authService)
    {
        _authService = authService;

    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuth request)
    {
        if (request == null || !ModelState.IsValid)
            return BadRequest("Email and password are required");

        var token = await _authService.login(request);

        if (token == null)
            return Unauthorized("Invalid email or password");

        return Ok(new {token});
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] UserAuth request)
    {

        if (string.IsNullOrEmpty(request.email))
            return BadRequest("Email is required.");

        if (string.IsNullOrEmpty(request.password)) return BadRequest("Password is required.");

        try
        {
            var newToken = await _authService.Refresh(request);
            if(newToken == null)
                return Unauthorized("Invalid email or password");

            return Ok(new { accessToken = newToken });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}
