namespace TableFlowBackend.Controllers;

using Microsoft.AspNetCore.Mvc;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

[ApiController]
[Route("api/[controller]")]
public class WaitlistController : ControllerBase
{
    private readonly WaitlistService _waitlistService;

    public WaitlistController(WaitlistService waitlistService)
    {
        _waitlistService = waitlistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWaitlistEntries()
    {
        var waitlist = await _waitlistService.GetAllWaitlistEntriesAsync();
        return Ok(waitlist);
    }

    [HttpPost]
    public async Task<IActionResult> AddToWaitlist([FromBody] Waitlist waitlistEntry)
    {
        try
        {
            await _waitlistService.AddToWaitlistAsync(waitlistEntry);
            return Ok(new { Message = "Added to waitlist." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("notify")]
    public async Task<IActionResult> NotifyNextCustomer()
    {
        var nextCustomer = await _waitlistService.NotifyNextCustomerAsync();
        if (nextCustomer == null)
            return NotFound("No customers on the waitlist.");

        return Ok(nextCustomer);
    }
}
