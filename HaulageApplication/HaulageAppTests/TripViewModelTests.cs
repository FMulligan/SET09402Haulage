using Moq;
using HaulageApp.ViewModels;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HaulageApp.Tests
{
    public class TripViewModelTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly DbContextOptions<HaulageDbContext> _options;

        public TripViewModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _options = new DbContextOptionsBuilder<HaulageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private void ClearDatabase()
        {
            using (var context = new HaulageDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task TripViewModel_LoadsDataAsync_ForDriver()
        {
            ClearDatabase();

            using (var context = new HaulageDbContext(_options))
            {
                context.trip.AddRange(
                    new Trip { Id = 1, StartTime = DateTime.Now.AddHours(-2), Status = "ongoing" },
                    new Trip { Id = 2, StartTime = DateTime.Now.AddHours(-4), Status = "completed" }
                );
                await context.SaveChangesAsync();
            }

            _userServiceMock.Setup(us => us.IsDriver()).Returns(true);

            var viewModel = new TripViewModel(new HaulageDbContext(_options), _userServiceMock.Object);

            await viewModel.LoadDataAsync();

            Assert.NotNull(viewModel.TripGroups);
            Assert.Equal(2, viewModel.TripGroups.Count);
            Assert.All(viewModel.TripGroups, t => Assert.True(t.IsEditButtonVisible));
        }

        [Fact]
        public async Task TripViewModel_LoadsDataAsync_ForAdmin()
        {
            ClearDatabase();

            using (var context = new HaulageDbContext(_options))
            {
                context.trip.AddRange(
                    new Trip { Id = 1, StartTime = DateTime.Now.AddHours(-2), Status = "ongoing" },
                    new Trip { Id = 2, StartTime = DateTime.Now.AddHours(-4), Status = "completed" }
                );
                await context.SaveChangesAsync();
            }

            _userServiceMock.Setup(us => us.IsDriver()).Returns(false);

            var viewModel = new TripViewModel(new HaulageDbContext(_options), _userServiceMock.Object);

            await viewModel.LoadDataAsync();

            Assert.NotNull(viewModel.TripGroups);
            Assert.Equal(2, viewModel.TripGroups.Count);
            Assert.All(viewModel.TripGroups, t => Assert.False(t.IsEditButtonVisible));
        }

        [Fact]
        public async Task TripViewModel_LoadsDataAsync_WhenDatabaseIsEmpty()
        {
            ClearDatabase();

            _userServiceMock.Setup(us => us.IsDriver()).Returns(true);

            var viewModel = new TripViewModel(new HaulageDbContext(_options), _userServiceMock.Object);

            await viewModel.LoadDataAsync();

            Assert.NotNull(viewModel.TripGroups);
            Assert.Empty(viewModel.TripGroups);
        }
    }
}
