using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HaulageAppTests;

public class EditCustomerTest
{
    private readonly Mock<HaulageDbContext> _mockContext;
    private readonly EditCustomerViewModel _viewModel;
    private readonly Mock<DbSet<User>> _mockUser;
    private Mock<INavigationService> _mockNavigationService;
    private static User activeCustomer { get; set; }
    private User inactiveCustomer;

    public EditCustomerTest()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockUser = new Mock<DbSet<User>>();
        _mockNavigationService = new();
        _viewModel = new EditCustomerViewModel(_mockContext.Object, _mockNavigationService.Object);
        activeCustomer = new User { Email = "customer", Password = "1234", Status = "active", Role = 1 };
        inactiveCustomer = new User { Email = "customer2", Password = "1234", Status = "inactive", Role = 1 };
    }
    
    [Fact]
    public void ApplyQueryAttributesShouldAssignCustomer()
    {
        var query = new Dictionary<string, object> { { "customer", activeCustomer } };
        _viewModel.ApplyQueryAttributes(query);
        Assert.Equal("customer", _viewModel.Email);
    }
    
    [Fact]
    public void ApplyQueryAttributesAssignsCorrectStatusIndexWhenActive()
    {
        var query = new Dictionary<string, object> { { "customer", activeCustomer } };
        _viewModel.ApplyQueryAttributes(query);
        Assert.Equal(0, _viewModel.StatusIndex);
    }
    
    [Fact]
    public void ApplyQueryAttributesAssignsCorrectStatusIndexWhenInactive()
    {
        var query = new Dictionary<string, object> { { "customer", inactiveCustomer } };
        _viewModel.ApplyQueryAttributes(query);
        Assert.Equal(1, _viewModel.StatusIndex);
    }
    
    [Fact]
    public async Task SaveShouldUpdateExistingCustomer()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "customer", inactiveCustomer } });
        _mockContext.Setup(e => e.user).Returns(_mockUser.Object);
        _mockContext.Setup(e => e.Entry(inactiveCustomer)).Returns(new Mock<FakeEntityEntry<User>>().Object);
        await _viewModel.SaveCommand.ExecuteAsync(null);
        _mockUser.Verify(c => c.AsQueryable(), Times.Once);
        _mockNavigationService.Verify(c => c.GoToAsync("..", new Dictionary<string, object>{{ "reload", "" }}));
    }
}