using Microsoft.AspNetCore.Mvc;
using TableFlowBackend.Services;

namespace TableFlowBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableAssignmentController : ControllerBase
{
    private readonly TableAssignmentService _tableAssignmentService;

    public TableAssignmentController(TableAssignmentService tableAssignmentService)
    {
        _tableAssignmentService = tableAssignmentService;
    }

    [HttpPost("assign/{reservationId}")]
    public async Task<IActionResult> AssignTable(int reservationId)
    {
        try
        {
            await _tableAssignmentService.AssignTableAsync(reservationId);
            return Ok(new { Message = "Table assigned successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
