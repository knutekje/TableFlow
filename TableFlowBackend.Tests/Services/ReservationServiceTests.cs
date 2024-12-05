using Moq;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

namespace TableFlowBackend.Tests.Services;

public class ReservationServiceTests
{
    [Fact]
    public async Task GetAllReservationsAsync_ShouldReturnAllReservations()
    {
        var mockRepo = new Mock<IRepository<Reservation>>();
        mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Reservation>
        {
            new Reservation { CustomerName = "Alice" },
            new Reservation { CustomerName = "Bob" }
        });

        var service = new ReservationService(mockRepo.Object);

        var reservations = await service.GetAllReservationsAsync();

        Assert.Equal(2, reservations.Count());
    }

    [Fact]
    public async Task AddReservationAsync_ShouldThrowException_WhenPartySizeIsZero()
    {
        var mockRepo = new Mock<IRepository<Reservation>>();
        var service = new ReservationService(mockRepo.Object);

        var reservation = new Reservation { PartySize = 0 };

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddReservationAsync(reservation));
    }

    [Fact]
    public async Task AddReservationAsync_ShouldCallRepository_WhenValidReservationIsAdded()
    {
        var mockRepo = new Mock<IRepository<Reservation>>();
        var service = new ReservationService(mockRepo.Object);

        var reservation = new Reservation { CustomerName = "Charlie", PartySize = 3 };

        await service.AddReservationAsync(reservation);

        mockRepo.Verify(repo => repo.AddAsync(reservation), Times.Once);
    }
}