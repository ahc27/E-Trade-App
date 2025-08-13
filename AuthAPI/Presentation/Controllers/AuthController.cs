using AuthAPI.Service;
using classLib;
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
        {
            Exception exception = new ArgumentException("Email and password is required.");
            var invalidLog = await _authService.LogAuth(null, false,"Login","Error occured in the connection between AuthApi and gateway"
                ,new ArgumentException("Email and password is required."));
            return BadRequest("Email and password are required");
        }

        var authResponse = await _authService.Login(request);

        if (authResponse == null)
        {
            bool failedLog = await _authService.LogAuth(null, false,"Login","User login failed", new ArgumentException("Email and password is required."));
            return Unauthorized("Invalid email or password");
        }



        return Ok(authResponse);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string request)
    {

        if (string.IsNullOrEmpty(request)) return null;

        try
        {
            var authResponse = await _authService.Refresh(request);
            if(authResponse == null)
                return Unauthorized("Invalid token");

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}
