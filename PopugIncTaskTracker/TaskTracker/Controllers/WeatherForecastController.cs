using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [Authorize]
    [HttpGet("test-auth")]
    public IActionResult Get()
    {
        return Ok();
    }
}