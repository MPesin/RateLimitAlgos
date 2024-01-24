using DosAssignment.RateLimiters.Exceptions;
using DosAssignment.RateLimiters.Managers;
using Microsoft.AspNetCore.Mvc;

namespace DosAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class StaticWindowController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] int clientId)
    {
        try
        {
            await RateLimiterService.SetStaticRequestLimitAsync(clientId);
            return Ok();
        }
        catch (RequestLimitReachedException)
        {
            return StatusCode(503);
        }
    }
}