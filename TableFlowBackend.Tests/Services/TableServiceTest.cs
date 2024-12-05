namespace TableFlowBackend.Tests.Services;

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TableFlowBackend.Models;
using TableFlowBackend;
using TableFlowBackend.Services;

public class TableServiceTests
{
    [Fact]
    public async Task GetAllTablesAsync_ShouldReturnAllTables()
    {
        var mockRepo = new Mock<IRepository<Table>>();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Table>
            {
                new Table { TableId = 1, Capacity = 4, IsAvailable = true },
                new Table { TableId = 2, Capacity = 2, IsAvailable = false }
            });

        var service = new TableService(mockRepo.Object);

        var tables = await service.GetAllTablesAsync();

        Assert.Equal(2, tables.Count());
    }

    [Fact]
    public async Task AddTableAsync_ShouldThrowException_WhenCapacityIsZeroOrLess()
    {
        var mockRepo = new Mock<IRepository<Table>>();
        var service = new TableService(mockRepo.Object);

        var table = new Table { Capacity = 0 };

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddTableAsync(table));
    }

    [Fact]
    public async Task AddTableAsync_ShouldAddTable_WhenValidTableIsProvided()
    {
        var mockRepo = new Mock<IRepository<Table>>();
        var service = new TableService(mockRepo.Object);

        var table = new Table { Capacity = 4, IsAvailable = true };

        await service.AddTableAsync(table);

        mockRepo.Verify(repo => repo.AddAsync(table), Times.Once);
    }
}
