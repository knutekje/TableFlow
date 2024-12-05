namespace TableFlowBackend.Tests.Services;

using Microsoft.AspNetCore.SignalR;
using TableFlowBackend.Hubs;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

public class WaitlistServiceTests
{
    [Fact]
    public async Task AddToWaitlist_ShouldAddCustomerToWaitlist_WhenValidRequestIsProvided()
    {
        
        var mockRepo = new Mock<IRepository<Waitlist>>();
        var mockHubContext = new Mock<IHubContext<NotificationHub>>();
        var service = new WaitlistService(mockRepo.Object, mockHubContext.Object);
        var waitlistEntry = new Waitlist
        {
            CustomerName = "Alice",
            RequestedTime = DateTime.UtcNow,
            PartySize = 2
        };

        
        await service.AddToWaitlistAsync(waitlistEntry);

        mockRepo.Verify(repo => repo.AddAsync(waitlistEntry), Times.Once);
    }

    [Fact]
    public async Task NotifyNextCustomer_ShouldReturnNextCustomer_WhenTableBecomesAvailable()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Waitlist>>();
        var mockHubContext = new Mock<IHubContext<NotificationHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        // Mock the repository
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Waitlist>
            {
                new Waitlist { WaitlistId = 1, CustomerName = "Alice", RequestedTime = DateTime.UtcNow.AddMinutes(-5) }
            });

        // Mock SignalR hub context
        mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);

        // Mock SendCoreAsync
        mockClientProxy.Setup(client => client.SendCoreAsync(
                It.IsAny<string>(), 
                It.IsAny<object[]>(), 
                default))
            .Returns(Task.CompletedTask);

        var service = new WaitlistService(mockRepo.Object, mockHubContext.Object);

        // Act
        var nextCustomer = await service.NotifyNextCustomerAsync();

        // Assert
        Assert.NotNull(nextCustomer);
        Assert.Equal("Alice", nextCustomer.CustomerName);

        // Verify notification
        mockClientProxy.Verify(client => client.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o.Contains("Table is now available for Alice.")),
            default), Times.Once);

        // Verify repository deletion
        mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }



    [Fact]
    public async Task NotifyNextCustomerAsync_ShouldSendNotificationAndRemoveCustomer()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Waitlist>>();
        var mockHubContext = new Mock<IHubContext<NotificationHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        // Mock the repository
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Waitlist>
            {
                new Waitlist { WaitlistId = 1, CustomerName = "Alice", RequestedTime = DateTime.UtcNow.AddMinutes(-5) }
            });

        // Mock SignalR's hub context
        mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);

        // Mock SendCoreAsync (instead of SendAsync)
        mockClientProxy.Setup(client => client.SendCoreAsync(
                It.IsAny<string>(), 
                It.IsAny<object[]>(), 
                default))
            .Returns(Task.CompletedTask);

        var service = new WaitlistService(mockRepo.Object, mockHubContext.Object);

        // Act
        var nextCustomer = await service.NotifyNextCustomerAsync();

        // Assert
        Assert.NotNull(nextCustomer);
        Assert.Equal("Alice", nextCustomer.CustomerName);

        // Verify that SendCoreAsync was called
        mockClientProxy.Verify(client => client.SendCoreAsync(
            "ReceiveNotification",
            It.Is<object[]>(o => o.Contains("Table is now available for Alice.")),
            default), Times.Once);

        // Verify that the customer was removed from the waitlist
        mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    
}
