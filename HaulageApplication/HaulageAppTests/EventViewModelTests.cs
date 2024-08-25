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

    public EventViewModelTests()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockEvents = new Mock<DbSet<Event>>();
        _viewModel = new EventViewModel(_mockContext.Object);
        _trip = new Trip { Id = 1, Driver = 2 }; 
        
        _mockContext.Setup(e => e.events).Returns(_mockEvents.Object);

    }
    [Fact]
    public void ShouldAssignEventWhenQureied()
    {
        var events = new Event { Id = 2, TripId = 1, EventType = "Delay", Timestamp = DateTime.Now};
        var query = new Dictionary<string, object> { { "trip", _trip }, { "event", events } };
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
        var events = new Event { Id = 2, TripId = 1, EventType = "Delay", Timestamp = DateTime.Now};
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "event", events } });
        _viewModel.EventType = "Emergency";
        
        _mockContext.Setup(e => e.Entry(events)).Returns(new Mock<FakeEntityEntry<Event>>().Object);
        _viewModel.Save();
        
        _mockEvents.Verify(c => c.AsQueryable(), Times.Once);
        _mockEvents.Verify(c => c.Add(It.IsAny<Event>()), Times.Never);
        return Task.CompletedTask;
    }

    [Fact] public Task DeleteShouldRemoveEvent() 
    { 
        var events = new Event { Id = 2, TripId = 1, EventType = "Delay2", Timestamp = DateTime.Now};
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "event", events } });
        
        _viewModel.Delete();
        
        _mockEvents.Verify(c => c.Remove(It.IsAny<Event>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        return Task.CompletedTask;
    }
}