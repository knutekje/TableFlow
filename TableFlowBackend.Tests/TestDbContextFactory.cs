using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Data;

namespace TableFlowBackend.Tests;

public class TestDbContextFactory
{
    public static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new AppDbContext(options);
    }
}