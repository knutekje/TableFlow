using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableFlowBackend.Models;
using TableFlowBackend.Services;
using Xunit;

public class ReservationServiceTests
{
    [Fact]
    public async Task GetAllReservationsAsync_ShouldReturnAllReservations()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Reservation>>();
        mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Reservation>
        {
            new Reservation { CustomerName = "Alice" },
            new Reservation { CustomerName = "Bob" }
        });

        var service = new ReservationService(mockRepo.Object);

        // Act
        var reservations = await service.GetAllReservationsAsync();

        // Assert
        Assert.Equal(2, reservations.Count());
    }

    [Fact]
    public async Task AddReservationAsync_ShouldThrowException_WhenPartySizeIsZero()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Reservation>>();
        var service = new ReservationService(mockRepo.Object);

        var reservation = new Reservation { PartySize = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddReservationAsync(reservation));
    }

    [Fact]
    public async Task AddReservationAsync_ShouldCallRepository_WhenValidReservationIsAdded()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Reservation>>();
        var service = new ReservationService(mockRepo.Object);

        var reservation = new Reservation { CustomerName = "Charlie", PartySize = 3 };

        // Act
        await service.AddReservationAsync(reservation);

        // Assert
        mockRepo.Verify(repo => repo.AddAsync(reservation), Times.Once);
    }
}