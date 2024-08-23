using System.Collections.ObjectModel;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace HaulageAppTests;

public class VehicleTests
{
    private readonly Mock<HaulageDbContext> _mockContext;
    private readonly Mock<DbSet<Vehicle>> _mockVehicle;
    
    public VehicleTests()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockVehicle = new Mock<DbSet<Vehicle>>();
        _mockContext.Setup(c => c.vehicle).Returns(_mockVehicle.Object);
    }

    [Fact]
    public Task SaveShouldAddVehicleToDbWhenNew()
    {
        var vehicle = new Vehicle
        {
            Id = 0,
            Type = "Van",
            Capacity = 500,
            Status = "available"
        };
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);
        viewModel.SaveCommand.Execute(null);
        _mockVehicle.Verify(c => c.Add(It.IsAny<Vehicle>()), Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        return Task.CompletedTask;
    }

    [Fact]
    public Task SaveShouldUpdateVehicleToDbWhenExists()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Van",
            Capacity = 500,
            Status = "available"
        };
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle)
        {
            Type="Van",
            Capacity = 50,
            Status="in use"
        };

        viewModel.SaveCommand.Execute(null);
        _mockVehicle.Verify(c => c.Add(It.IsAny<Vehicle>()), Times.Never);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        return Task.CompletedTask;
    }

    [Fact]
    public Task DeleteShouldRemoveVehicle()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Van",
            Capacity = 500,
            Status = "available"
        };
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);
        viewModel.DeleteCommand.Execute(null);
        
        _mockVehicle.Verify(c => c.Remove(It.IsAny<Vehicle>()), Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        return Task.CompletedTask;
    }

    [Fact]
    public void ReloadShouldReloadContext()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Van",
            Capacity = 500,
            Status = "available"
        };
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);
        _mockContext.Setup(c => c.Entry(vehicle)).Returns(new Mock<FakeEntityEntry<Vehicle>>().Object);
        viewModel.Reload();
        
        _mockContext.Verify(c => c.Entry(It.IsAny<Vehicle>()).Reload(), Times.Once);
        
    }
}