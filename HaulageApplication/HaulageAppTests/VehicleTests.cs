using System.Collections.ObjectModel;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
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
    }

    [Fact]
    public async Task SaveShouldAddVehicleToDbWhenNew()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Van",
            Capacity = 500,
            Status = "available"
        };
        _mockContext.Setup(c => c.vehicle).Returns(_mockVehicle.Object);
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);

        await viewModel.SaveCommand.ExecuteAsync(null);
        _mockVehicle.Verify(c => c.Add(It.IsAny<Vehicle>()), Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        
    }

    [Fact]
    public async Task SaveShouldUpdateVehicleToDbWhenExists()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Truck",
            Capacity = 500,
            Status = "available"
        };
        _mockContext.Setup(c => c.vehicle).Returns(_mockVehicle.Object);
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle)
        {
            Type="Van",
            Capacity = 50,
            Status="in use"
        };

        await viewModel.SaveCommand.ExecuteAsync(null);
        _mockVehicle.Verify(c => c.Add(It.IsAny<Vehicle>()), Times.Never);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task DeleteShouldRemoveVehicle()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Truck",
            Capacity = 500,
            Status = "available"
        };
        _mockContext.Setup(c => c.vehicle).Returns(_mockVehicle.Object);
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);
        
        await viewModel.SaveCommand.ExecuteAsync(null);
        _mockVehicle.Verify(c => c.Remove(It.IsAny<Vehicle>()), Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task ReloadShouldReloadContext()
    {
        var vehicle = new Vehicle
        {
            Id = 1,
            Type = "Truck",
            Capacity = 500,
            Status = "available"
        };
        _mockContext.Setup(c => c.vehicle).Returns(_mockVehicle.Object);
        var viewModel = new VehicleViewModel(_mockContext.Object, vehicle);
        
        viewModel.Reload();
        
        _mockContext.Verify(c => c.Entry(It.IsAny<Vehicle>()).Reload(), Times.Once);
        
    }

}