
using TableFlowBackend.Models;

namespace TableFlowBackend.Services;

public class ReservationService
{
    private readonly IRepository<Reservation> _reservationRepository;

    public ReservationService(IRepository<Reservation> reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _reservationRepository.GetAllAsync();
    }

    public async Task<Reservation?> GetReservationByIdAsync(int id)
    {
        return await _reservationRepository.GetByIdAsync(id);
    }

    public async Task AddReservationAsync(Reservation reservation)
    {
        if (reservation.PartySize <= 0)
            throw new ArgumentException("Party size must be greater than zero.");

        await _reservationRepository.AddAsync(reservation);
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        if (reservation.PartySize <= 0)
            throw new ArgumentException("Party size must be greater than zero.");

        await _reservationRepository.UpdateAsync(reservation);
    }

    public async Task DeleteReservationAsync(int id)
    {
        await _reservationRepository.DeleteAsync(id);
    }
}