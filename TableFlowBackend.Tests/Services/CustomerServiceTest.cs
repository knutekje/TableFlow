namespace TableFlowBackend.Tests.Services;

using Moq;
using System.Threading.Tasks;
using Xunit;
using TableFlowBackend.Models;
using TableFlowBackend.Services;

public class CustomerServiceTests
{
    [Fact]
    public async Task AddCustomerAsync_ShouldAddCustomer_WhenValidCustomerIsProvided()
    {
        var mockRepo = new Mock<IRepository<Customer>>();
        var service = new CustomerService(mockRepo.Object);

        var customer = new Customer { Name = "John Doe", Email = "john@example.com" };

        await service.AddCustomerAsync(customer);

        mockRepo.Verify(repo => repo.AddAsync(customer), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        var mockRepo = new Mock<IRepository<Customer>>();
        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(new Customer { CustomerId = 1, Name = "John Doe" });

        var service = new CustomerService(mockRepo.Object);

        var customer = await service.GetCustomerByIdAsync(1);

        Assert.NotNull(customer);
        Assert.Equal("John Doe", customer.Name);
    }

    [Fact]
    public async Task DeleteCustomerAsync_ShouldDeleteCustomer_WhenCustomerExists()
    {
        var mockRepo = new Mock<IRepository<Customer>>();
    
        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(new Customer { CustomerId = 1, Name = "John Doe" });
    
        mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        var service = new CustomerService(mockRepo.Object);

        await service.DeleteCustomerAsync(1);

        mockRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once); // Verify that GetByIdAsync was called
        mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);  // Verify that DeleteAsync was called
    }
    
    [Fact]
    public async Task DeleteCustomerAsync_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        var mockRepo = new Mock<IRepository<Customer>>();
    
        mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer?)null);

        var service = new CustomerService(mockRepo.Object);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteCustomerAsync(1));
        Assert.Equal("Customer with ID 1 not found.", exception.Message);

        mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
    }


}
