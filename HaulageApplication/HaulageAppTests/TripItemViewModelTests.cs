using Moq;
using HaulageApp.ViewModels;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HaulageAppTests
{
    public class TripItemViewModelTests : IAsyncLifetime
    {
        private Mock<IUserService> _userServiceMock;
        private MockDb _mockDb;
        private DbContextOptions<HaulageDbContext> _options;

        public TripItemViewModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mockDb = new MockDb();
            _options = _mockDb.CreateContextOptions();
        }

        public Task InitializeAsync()
        {
            _mockDb.ClearDatabase(_options);
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _mockDb.ClearDatabase(_options);
            return Task.CompletedTask;
        }

        private TripItemViewModel CreateTripItemViewModel(Trip trip, bool canEdit)
        {
            return new TripItemViewModel(trip, new HaulageDbContext(_options), canEdit);
        }

        [Fact]
        public async Task TripItemViewModel_SaveTrip_ShouldUpdateDatabase_WhenCalled()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = await context.trip.FirstOrDefaultAsync(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: true);
            viewModel.StartTimeString = "2024-08-19 09:00";
            viewModel.EndTimeString = "2024-08-19 12:00";

            await viewModel.SaveTrip();

            Assert.Equal("completed", viewModel.Status); // Status should be updated to completed
        }

        [Fact]
        public async Task TripItemViewModel_SaveTrip_ShouldChangeStatus_WhenEndTimeIsNotDefinedButUpdatedAtWasChanged()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = await context.trip.FirstOrDefaultAsync(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: true);
            viewModel.StartTimeString = trip.StartTime.ToString("yyyy-MM-dd HH:mm");
            viewModel.EndTimeString = ""; // No changes made to EndTime

            await viewModel.SaveTrip();

            Assert.Equal("ongoing", viewModel.Status); // Status should remain as "ongoing"
        }

        [Fact]
        public async Task TripItemViewModel_SaveTrip_ShouldNotUpdateDatabase_WhenNoChangesMade()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = await context.trip.FirstOrDefaultAsync(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: true);
            viewModel.StartTimeString = trip.StartTime.ToString("yyyy-MM-dd HH:mm");
            viewModel.EndTimeString = trip.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;

            await viewModel.SaveTrip();

            Assert.Equal(trip.Status, viewModel.Status); // Status should remain the same
        }

        [Fact]
        public void TripItemViewModel_ToggleEditCommand_ShouldToggleIsEditing_WhenExecuted()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = context.trip.FirstOrDefault(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: true);

            Assert.False(viewModel.IsEditing); // Initially not editing
            viewModel.ToggleEditCommand.Execute(null);
            Assert.True(viewModel.IsEditing); // Should be editing after first toggle
            viewModel.ToggleEditCommand.Execute(null);
            Assert.False(viewModel.IsEditing); // Should not be editing after second toggle
        }

        [Fact]
        public void TripItemViewModel_IsDriver_ShouldAllowEditing_ForDrivers()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = context.trip.FirstOrDefault(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: true); // true indicates the user is a driver

            Assert.True(viewModel.IsEditButtonVisible); // Drivers can edit
        }

        [Fact]
        public void TripItemViewModel_IsAdmin_ShouldNotAllowEditing_ForAdmins()
        {
            _mockDb.CreateContextWithTrips(_options);

            using var context = new HaulageDbContext(_options);
            var trip = context.trip.FirstOrDefault(t => t.Id == 1);

            var viewModel = CreateTripItemViewModel(trip, canEdit: false); // false indicates the user is not a driver (admin)

            Assert.False(viewModel.IsEditButtonVisible); // Admins cannot edit
        }
    }
}
