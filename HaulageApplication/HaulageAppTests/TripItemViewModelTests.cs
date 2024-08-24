using Moq;
using HaulageApp.ViewModels;
using HaulageApp.Data;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.Tests
{
    public class TripItemViewModelTests
    {
        [Fact]
        public async Task SaveTrip_ShouldUpdateDatabase_WhenCalled()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = null,
                Status = "ongoing",
                UpdatedAt = DateTime.Now
            };
            var viewModel = new TripItemViewModel(trip, mockContext.Object, true)
            {
                StartTimeString = "2024-08-19 09:00",
                EndTimeString = "2024-08-19 12:00"
            };

            await viewModel.SaveTrip();

            Assert.Equal("completed", viewModel.Status); // Status should be updated to completed
        }

        [Fact]
        public async Task SaveTrip_ShouldChangeStatus_WhenEndTimeIsNotDefinedButUpdatedAtWasChanged()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = null, // No end time
                Status = "planned",
                UpdatedAt = DateTime.Now
            };
            
            var viewModel = new TripItemViewModel(trip, mockContext.Object, true)
            {
                StartTimeString = trip.StartTime.ToString("yyyy-MM-dd HH:mm"),
                EndTimeString = "" // No changes made to EndTime
            };
            await viewModel.SaveTrip();
            
            Assert.Equal("ongoing", viewModel.Status); // Status should remain as "planned"
        }

        [Fact]
        public async Task SaveTrip_ShouldNotUpdateDatabase_WhenNoChangesMade()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = DateTime.Parse("2024-08-19 12:00"),
                Status = "completed",
                UpdatedAt = DateTime.Now
            };

            mockContext.Setup(c => c.Attach(It.IsAny<Trip>())).Verifiable();
            mockContext.Setup(c => c.Update(It.IsAny<Trip>())).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var viewModel = new TripItemViewModel(trip, mockContext.Object, true)
            {
                StartTimeString = "2024-08-19 08:00",
                EndTimeString = "2024-08-19 12:00"
            };
            await viewModel.SaveTrip();

            mockContext.Verify(c => c.Attach(It.IsAny<Trip>()), Times.Never);
            mockContext.Verify(c => c.Update(It.IsAny<Trip>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
        
        [Fact]
        public void ToggleEditCommand_ShouldToggleIsEditing_WhenExecuted()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = DateTime.Parse("2024-08-19 12:00"),
                Status = "completed",
                UpdatedAt = DateTime.Now
            };

            var viewModel = new TripItemViewModel(trip, mockContext.Object, true);

            Assert.False(viewModel.IsEditing); // Initially not editing
            viewModel.ToggleEditCommand.Execute(null);
            Assert.True(viewModel.IsEditing); // Should be editing after first toggle
            viewModel.ToggleEditCommand.Execute(null);
            Assert.False(viewModel.IsEditing); // Should not be editing after second toggle
        }

        [Fact]
        public void TripItemViewModel_IsDriver_ShouldAllowEditing_ForDrivers()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = DateTime.Parse("2024-08-19 12:00"),
                Status = "completed",
                UpdatedAt = DateTime.Now
            };

            var viewModel = new TripItemViewModel(trip, mockContext.Object, true); // true indicates the user is a driver

            Assert.True(viewModel.IsEditButtonVisible); // Drivers can edit
        }

        [Fact]
        public void TripItemViewModel_IsAdmin_ShouldNotAllowEditing_ForAdmins()
        {
            var mockContext = new Mock<HaulageDbContext>(new DbContextOptions<HaulageDbContext>());
            var trip = new Trip
            {
                Id = 1,
                StartTime = DateTime.Parse("2024-08-19 08:00"),
                EndTime = DateTime.Parse("2024-08-19 12:00"),
                Status = "completed",
                UpdatedAt = DateTime.Now
            };

            var viewModel = new TripItemViewModel(trip, mockContext.Object, false); // false indicates the user is not a driver (admin)

            Assert.False(viewModel.IsEditButtonVisible); // Admins cannot edit
        }
    }
}
