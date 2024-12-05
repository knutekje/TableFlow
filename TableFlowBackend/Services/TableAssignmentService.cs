using TableFlowBackend.Models;

namespace TableFlowBackend.Services;

public class TableAssignmentService
{
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<Table> _tableRepository;

    public TableAssignmentService(IRepository<Reservation> reservationRepository, IRepository<Table> tableRepository)
    {
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
    }

    public async Task AssignTableAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            throw new KeyNotFoundException($"Reservation with ID {reservationId} not found.");

        var availableTables = await _tableRepository.GetAllAsync();
        var suitableTable = availableTables.FirstOrDefault(t => t.IsAvailable && t.Capacity >= reservation.PartySize);

        if (suitableTable == null)
            throw new InvalidOperationException("No suitable table available.");

        // Assign the table
        suitableTable.IsAvailable = false;
        reservation.TableId = suitableTable.TableId;

        // Update the table and reservation
        await _tableRepository.UpdateAsync(suitableTable);
        await _reservationRepository.UpdateAsync(reservation);
    }
}
