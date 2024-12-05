
using Microsoft.Extensions.Logging;

namespace TableFlowBackend.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Moq;
using TableFlowBackend.Services;



using Microsoft.EntityFrameworkCore;
using TableFlowBackend.Data;
using TableFlowBackend.Models;
using Xunit;

public class ReservationControllerTests
{
    
    [Fact]
    public async Task GetReservationById_ShouldReturnNotFound_WhenReservationDoesNotExist()
    {

        var mockRepo = new Mock<IRepository<Reservation>>();
        mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Reservation?)null);

        var service = new ReservationService(mockRepo.Object); 
        var controller = new ReservationController(service, null);   

        var result = await controller.GetReservationById(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("404", notFoundResult.StatusCode.ToString());
    }

    [Fact]
    public async Task CreateReservation_ShouldReturnCreated_WhenReservationIsCreated()
    {
        
        
        var mockRepo = new Mock<IRepository<Reservation>>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Reservation>()))
            .Returns(Task.CompletedTask);
        
        var service = new ReservationService(mockRepo.Object);
        var controller = new ReservationController(service, null);
       
        
        var result = await controller.AddReservation(new Reservation { CustomerName = "Alice", PartySize = 1});
        
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetReservationById", createdResult.ActionName);
        
        
    }


}