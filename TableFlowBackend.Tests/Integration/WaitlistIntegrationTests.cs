using TableFlowBackend.Hubs;

namespace TableFlowBackend.Tests.Integration;

using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

public class WaitlistIntegrationTests
{
    [Fact]
    public async Task NotifyNextCustomer_ShouldNotifyAndRemoveCustomerFromWaitlist()
    {
        // Arrange
        var mockHubContext = new Mock<IHubContext<NotificationHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockHubContext.Setup(h => h.Clients).Returns(mockClients.Object);
        mockClients.Setup(c => c.All).Returns(mockClientProxy.Object);

        using var context = TestDbContextFactory.CreateInMemoryContext();
        var waitlistRepo = new Repository<Waitlist>(context);
        var service = new WaitlistService(waitlistRepo, mockHubContext.Object);

        // Seed test data
        var waitlistEntry = new Waitlist
        {
            WaitlistId = 1,
            CustomerName = "Alice",
            RequestedTime = DateTime.UtcNow.AddMinutes(-10)
        };
        await waitlistRepo.AddAsync(waitlistEntry);

        // Act
        var notifiedCustomer = await service.NotifyNextCustomerAsync();

        // Assert
        Assert.NotNull(notifiedCustomer);
        Assert.Equal("Alice", notifiedCustomer.CustomerName);

        mockClientProxy.Verify(
            client => client.SendCoreAsync(
                "ReceiveNotification",
                It.Is<object[]>(o => o.Contains("Table is now available for Alice.")),
                default),
            Times.Once);

        var remainingWaitlist = await waitlistRepo.GetAllAsync();
        Assert.Empty(remainingWaitlist);
    }
}
