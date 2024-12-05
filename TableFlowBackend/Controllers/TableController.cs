using Microsoft.AspNetCore.Mvc;
using TableFlowBackend.Services;

namespace TableFlowBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly TableService _tableService;

    public TableController(TableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTables()
    {
        var tables = await _tableService.GetAllTablesAsync();
        return Ok(tables);
    }

    [HttpPost]
    public async Task<IActionResult> AddTable([FromBody] Table table)
    {
        await _tableService.AddTableAsync(table);
        return CreatedAtAction(nameof(GetTables), new { id = table.TableId }, table);
    }
}
