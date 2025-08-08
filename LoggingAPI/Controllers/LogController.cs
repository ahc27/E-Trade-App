using classLib.LogDtos;
using LoggingAPI.Business.Service;
using Microsoft.AspNetCore.Mvc;

namespace LoggingAPI.Controllers
{
    // Only for demonstration , not take place in production
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _logService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> LogAuth([FromBody] Log request)
        {
            if (request == null) return BadRequest("Invalid log data");

            await _logService.LogAuthAsync(request);
            return Ok("Log saved successfully");

        }
    }
}

