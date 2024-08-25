using System.Runtime.CompilerServices;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HaulageAppTests;


public class EventViewModelTests
{
    private readonly Mock<HaulageDbContext> _mockContext;
    private readonly EventViewModel _viewModel;
    private readonly Mock<DbSet<Event>> _mockEvents;
    private readonly Trip _trip;
    private readonly Event _event;

    public EventViewModelTests()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockEvents = new Mock<DbSet<Event>>();
        _viewModel = new EventViewModel(_mockContext.Object);
        _trip = new Trip { Id = 1, Driver = 2 }; 
        _event = new Event { Id = 2, TripId = 1, EventType = "Delay", Timestamp = DateTime.Now};
        
        _mockContext.Setup(e => e.events).Returns(_mockEvents.Object);

    }
    [Fact]
    public void ShouldAssignEventWhenQureied()
    {
        var query = new Dictionary<string, object> { { "trip", _trip }, { "event", _event } };
        _viewModel.ApplyQueryAttributes(query);
        
        Assert.Equal("Delay", _viewModel.EventType);
    }
    
    [Fact] public Task SaveShouldAddNewEventIfEventIsNull()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "trip", _trip } }); 
        _viewModel.EventType = "Delay";
        
        _viewModel.Save();
        
        _mockEvents.Verify(c => c.Add(It.Is<Event>(e => e.TripId == 1 && e.EventType == "Delay")), Times.Once); 
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task SaveShouldUpdateEventIfExisting()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "event", _event } });
        _viewModel.EventType = "Emergency";
        
        _mockContext.Setup(e => e.Entry(_event)).Returns(new Mock<FakeEntityEntry<Event>>().Object);
        _viewModel.Save();
        
        _mockEvents.Verify(c => c.AsQueryable(), Times.Once);
        _mockEvents.Verify(c => c.Add(It.IsAny<Event>()), Times.Never);
        return Task.CompletedTask;
    }

    [Fact] public Task DeleteShouldRemoveEvent() 
    { 
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "event", _event } });
        
        _viewModel.Delete();
        
        _mockEvents.Verify(c => c.Remove(It.IsAny<Event>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        return Task.CompletedTask;
    }
}