using DosAssignment.RateLimiters.Exceptions;
using DosAssignment.RateLimiters.Managers;
using Microsoft.AspNetCore.Mvc;

namespace DosAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class DynamicWindowController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] int clientId)
    {
        try
        {
            await RateLimiterService.SetDynamicRequestLimitAsync(clientId);
            return Ok();
        }
        catch (RequestLimitReachedException)
        {
            return StatusCode(503);
        }
    }
}