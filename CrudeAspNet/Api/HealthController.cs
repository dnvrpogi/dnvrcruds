using Microsoft.AspNetCore.Mvc;

namespace CrudeAspNet.Api;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok", app = "StudentHub API" });
}