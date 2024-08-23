using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace HaulageAppTests
{
    public class AllBillsViewModelTests
    {
        private readonly DbContextOptions<HaulageDbContext> _options;
        private readonly Mock<ILogger<AllBillsViewModel>> _loggerMock;

        public AllBillsViewModelTests()
        {
            _options = new DbContextOptionsBuilder<HaulageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for each test
                .Options;
            _loggerMock = new Mock<ILogger<AllBillsViewModel>>();
        }

        private void ClearDatabase()
        {
            using (var context = new HaulageDbContext(_options))
            {
                context.Database.EnsureDeleted(); // Clears the database
                context.Database.EnsureCreated(); // Recreates the schema
            }
        }

        [Fact]
        public void AllBillsViewModel_LoadsSingleBillForCurrentUser()
        {
            // Arrange
            ClearDatabase();
            var userId = 1;

            using (var context = new HaulageDbContext(_options))
            {
                context.bill.AddRange(
                    new Bill { BillId = 1, CustomerId = userId, Amount = 100, Status = "paid", Items = new List<Item>() },
                    new Bill { BillId = 2, CustomerId = 2, Amount = 200, Status = "pending", Items = new List<Item>() }
                );
                context.SaveChanges();
            }

            using (var context = new HaulageDbContext(_options))
            {
                var viewModelMock = new Mock<AllBillsViewModel>(context, _loggerMock.Object) { CallBase = true };
                viewModelMock.Protected().Setup<int>("GetCurrentUserId").Returns(userId);

                var viewModel = viewModelMock.Object;

                // Assert
                Assert.Single(viewModel.AllBills);
                Assert.Equal(100, viewModel.AllBills[0].Amount);
            }
        }

        [Fact]
        public void AllBillsViewModel_LoadsNoBillsForCustomerWithNoBills()
        {
            // Arrange
            ClearDatabase();
            var userId = 2;

            using (var context = new HaulageDbContext(_options))
            {
                context.bill.AddRange(
                    new Bill { BillId = 1, CustomerId = 1, Amount = 100, Status = "paid", Items = new List<Item>() }
                );
                context.SaveChanges();
            }

            using (var context = new HaulageDbContext(_options))
            {
                var viewModelMock = new Mock<AllBillsViewModel>(context, _loggerMock.Object) { CallBase = true };
                viewModelMock.Protected().Setup<int>("GetCurrentUserId").Returns(userId);

                var viewModel = viewModelMock.Object;

                // Assert
                Assert.Empty(viewModel.AllBills); // No bills should be loaded for user ID 2
            }
        }

        [Fact]
        public void AllBillsViewModel_LoadsNoBillsWhenDatabaseIsEmpty()
        {
            // Arrange
            ClearDatabase();
            var userId = 3;

            using (var context = new HaulageDbContext(_options))
            {
                var viewModelMock = new Mock<AllBillsViewModel>(context, _loggerMock.Object) { CallBase = true };
                viewModelMock.Protected().Setup<int>("GetCurrentUserId").Returns(userId);

                var viewModel = viewModelMock.Object;

                // Assert
                Assert.Empty(viewModel.AllBills); // No bills should be loaded because the database is empty
            }
        }
    }
}
