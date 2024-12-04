using Microsoft.AspNetCore.Mvc;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> GetReservationById(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> AddReservation([FromBody] Reservation reservation)
    {
        await _reservationService.AddReservationAsync(reservation);
        return CreatedAtAction(nameof(GetReservations), new { id = reservation.ReservationId }, reservation);
    }
}