using Microsoft.AspNetCore.Mvc;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(ReservationService reservationService, ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<ReservationController>.Instance;    }

   

    [HttpGet("/api/[controller]/[action]")]
    public async Task<IActionResult> GetReservations()
    {
        try
        {
            _logger.LogInformation("Fetching all reservations");

            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetReservationById(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
        {
            return NotFound("Reservation not found");
        }
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> AddReservation([FromBody] Reservation reservation)
    {
        try
        {
            _logger.LogInformation("Adding a new reservation for {CustomerName}", reservation.CustomerName);
            await _reservationService.AddReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.ReservationId }, reservation);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}