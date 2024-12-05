using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Data;
using TableFlowBackend.Models;

namespace TableFlowBackend.Tests;

/*public class AppDBContextTest
{
    public string CustomerName { get; set; }

    var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        using var context = new AppDbContext(options);

// Add data
    context.Reservations.Add(new Reservation { CustomerName = "Alice" });
    await context.SaveChangesAsync();

// Verify data
    var reservation = await context.Reservations.FirstOrDefaultAsync();
    Assert.NotNull(reservation);
    Assert.Equal("Alice", reservation.CustomerName);

}*/