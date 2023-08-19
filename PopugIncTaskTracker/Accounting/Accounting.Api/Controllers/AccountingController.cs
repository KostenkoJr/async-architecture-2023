using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountingController : ControllerBase
{
    private readonly ILogger<AccountingController> _logger;

    public AccountingController(ILogger<AccountingController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}