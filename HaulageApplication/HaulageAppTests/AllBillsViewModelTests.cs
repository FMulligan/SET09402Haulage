using HaulageApp.Data;
using HaulageApp.Services;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace HaulageAppTests
{
    public class AllBillsViewModelTests
    {
        private readonly DbContextOptions<HaulageDbContext> _options;
        private readonly Mock<IBillService> _billServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AllBillsViewModel>> _loggerMock;

        public AllBillsViewModelTests()
        {
            _options = new DbContextOptionsBuilder<HaulageDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _billServiceMock = new Mock<IBillService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AllBillsViewModel>>();
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
        public async Task AllBillsViewModel_LoadsSingleBillForCurrentUser()
        {
            ClearDatabase();
            var userId = 1;

            var bills = new List<Bill>
            {
                SharedTestSetup.CreateBillWithItems(1, 100, "paid")
            };

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(userId);
            _billServiceMock.Setup(bs => bs.GetBillsForCurrentUserAsync(userId)).ReturnsAsync(bills);

            var viewModel = new AllBillsViewModel(_billServiceMock.Object, _userServiceMock.Object, _loggerMock.Object);

            await viewModel.LoadBillsAsync();

            Assert.Single(viewModel.AllBills);
            Assert.Equal(100, viewModel.AllBills[0].Amount);
        }

        [Fact]
        public async Task AllBillsViewModel_LoadsNoBillsForCustomerWithNoBills()
        {
            ClearDatabase();
            var userId = 2;

            var bills = new List<Bill>();

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(userId);
            _billServiceMock.Setup(bs => bs.GetBillsForCurrentUserAsync(userId)).ReturnsAsync(bills);

            var viewModel = new AllBillsViewModel(_billServiceMock.Object, _userServiceMock.Object, _loggerMock.Object);
            
            await viewModel.LoadBillsAsync();
            
            Assert.Empty(viewModel.AllBills);
        }

        [Fact]
        public async Task AllBillsViewModel_LoadsNoBillsWhenDatabaseIsEmpty()
        {
            ClearDatabase();
            var userId = 3;

            var bills = new List<Bill>();

            _userServiceMock.Setup(us => us.GetCurrentUserId()).Returns(userId);
            _billServiceMock.Setup(bs => bs.GetBillsForCurrentUserAsync(userId)).ReturnsAsync(bills);

            var viewModel = new AllBillsViewModel(_billServiceMock.Object, _userServiceMock.Object, _loggerMock.Object);

            await viewModel.LoadBillsAsync();

            Assert.Empty(viewModel.AllBills);
        }
    }
}
