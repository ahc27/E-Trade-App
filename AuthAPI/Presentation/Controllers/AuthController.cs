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
        var token = await _authService.Login(request);


        if (token == null)
        {
            bool failedLog = await _authService.LogAuth(null, false,"Login","User login failed", new ArgumentException("Email and password is required."));
            return Unauthorized("Invalid email or password");
        }

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(60)
        });
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
