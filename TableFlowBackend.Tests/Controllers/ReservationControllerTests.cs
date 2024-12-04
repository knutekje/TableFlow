using Microsoft.AspNetCore.Mvc;
using Moq;
using TableFlowBackend.Services;

namespace TableFlowBackend.Tests.Controllers;


using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Data;
using TableFlowBackend.Models;
using Xunit;

public class ReservationRepositoryTests
{
    [Fact]
    public async Task GetReservationById_ShouldReturnNotFound_WhenReservationDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ReservationService>(null);
        mockService.Setup(service => service.GetReservationByIdAsync(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());
        var controller = new ReservationController(mockService.Object);

        // Act
        var result = await controller.GetReservationById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

}