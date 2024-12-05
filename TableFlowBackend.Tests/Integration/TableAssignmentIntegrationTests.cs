namespace TableFlowBackend.Tests.Integration;

using Microsoft.EntityFrameworkCore;
using Xunit;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

public class TableAssignmentIntegrationTests
{
    [Fact]
    public async Task AssignTable_ShouldAssignAvailableTableToReservation()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryContext();

        var tableRepo = new Repository<Table>(context);
        var reservationRepo = new Repository<Reservation>(context);
        var service = new TableAssignmentService(reservationRepo, tableRepo);

        // Seed test data
        var table = new Table { TableId = 1, Capacity = 4, IsAvailable = true };
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerName = "John Doe", // Provide the required property
            ReservationDate = DateTime.UtcNow,
            PartySize = 4
        };

        await tableRepo.AddAsync(table);
        await reservationRepo.AddAsync(reservation);

        // Act
        await service.AssignTableAsync(1);

        // Assert
        var updatedReservation = await reservationRepo.GetByIdAsync(1);
        var updatedTable = await tableRepo.GetByIdAsync(1);

        Assert.NotNull(updatedReservation.TableId);
        Assert.Equal(1, updatedReservation.TableId);
        Assert.False(updatedTable.IsAvailable);
    }

}
