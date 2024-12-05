using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Data;
using TableFlowBackend.Models;

namespace TableFlowBackend.Tests.Repositories;

public class ReservationRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_GetAll")
            .Options;

        using var context = new AppDbContext(options);
        context.Reservations.Add(new Reservation { CustomerName = "Alice" });
        context.Reservations.Add(new Reservation { CustomerName = "Bob" });
        await context.SaveChangesAsync();

        var repository = new Repository<Reservation>(context);

        // Act
        var reservations = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, reservations.Count());
    }
    [Fact]
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_Add")
            .Options;

        using var context = new AppDbContext(options);
        var repository = new Repository<Reservation>(context);

        var reservation = new Reservation
        {
            CustomerName = "Charlie",
            ReservationDate = DateTime.UtcNow,
            PartySize = 4
        };

        // Act
        await repository.AddAsync(reservation);
        var reservations = await context.Reservations.ToListAsync();

        // Assert
        Assert.Single(reservations);
        Assert.Equal("Charlie", reservations[0].CustomerName);
    }
    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntityFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_Delete")
            .Options;

        using var context = new AppDbContext(options);
        context.Reservations.Add(new Reservation { ReservationId = 1, CustomerName = "David" });
        await context.SaveChangesAsync();

        var repository = new Repository<Reservation>(context);

        // Act
        await repository.DeleteAsync(1);
        var reservations = await context.Reservations.ToListAsync();

        // Assert
        Assert.Empty(reservations);
    }



}