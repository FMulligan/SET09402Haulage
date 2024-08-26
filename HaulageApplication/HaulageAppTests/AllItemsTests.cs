using System.Collections.ObjectModel;
using HaulageApp.Data;
using HaulageApp.Services;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace HaulageAppTests;

public class AllItemsTests
{
    private readonly MockDb _mockDb = new();
    private readonly DbContextOptions<HaulageDbContext> _options;
    private readonly HaulageDbContext _context;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IBillService> _billServiceMock;
    private readonly Mock<ILogger<AllItemsViewModel>> _loggerMock;
    private readonly AllItemsViewModel _viewModel;

    public AllItemsTests()
    {
        _options = _mockDb.CreateContextOptions();
        _mockDb.CreateContextWithTrips(_options);
        _context = new HaulageDbContext(_options);

        _userServiceMock = new Mock<IUserService>();
        _billServiceMock = new Mock<IBillService>();
        _loggerMock = new Mock<ILogger<AllItemsViewModel>>();

        _viewModel = new AllItemsViewModel(_context, _userServiceMock.Object, _billServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public Task DbShouldReturnAllItemsWhenLoaded()
    {
        Assert.Equal(4, _context.item.Count());
        return Task.CompletedTask;
    }

    [Fact]
    public Task GetLastestTripShouldReturnLatestTrip()
    {
        var tripId = _viewModel.GetLatestTripId();
        Assert.Equal(2, tripId);
        return Task.CompletedTask;
    }
}