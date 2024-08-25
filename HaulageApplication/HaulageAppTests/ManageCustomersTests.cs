using HaulageApp.Data;
using HaulageApp.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace HaulageAppTests;

public class ManageCustomersTests
{
    private readonly MockDb _db = new();
    private readonly HaulageDbContext _context;
    private readonly ManageCustomersViewModel _viewModel;
    
    public ManageCustomersTests()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);
        _context = new HaulageDbContext(options); 
        _viewModel = new ManageCustomersViewModel(_context);
    }
    
    [Fact] public Task ViewShouldReturnAllCustomersOnLoad()
    {
            Assert.True(_viewModel.Customers.Count == 3);
            return Task.CompletedTask;
    }
    
    [Fact] public Task SearchShouldReturnCustomerForExistingUserEmail()
    {
            _viewModel.CustomerIdOrEmail = "customer";
            _viewModel.SearchCommand.Execute(null);
            Assert.True(_viewModel.Customers.Count == 1 && _viewModel.Customers.FirstOrDefault()?.Email == "customer");
            return Task.CompletedTask;
    }
    
    [Fact] public Task SearchShouldReturnCustomerForExistingUserId()
    {
            _viewModel.CustomerIdOrEmail = "4";
            _viewModel.SearchCommand.Execute(null);
            Assert.True(_viewModel.Customers[0].Id == 4);
            return Task.CompletedTask;
    }
    
    [Fact] public Task SearchShouldNotReturnCustomerForInvalidUserEmail()
    {
            _viewModel.CustomerIdOrEmail = "whoIsThis"; // doesn't exist
            _viewModel.SearchCommand.Execute(null);
            Assert.True(_viewModel.Customers.IsNullOrEmpty());
            return Task.CompletedTask;
    }
    
    [Fact] public Task SearchShouldNotReturnCustomerForInvalidUserId()
    {
            _viewModel.CustomerIdOrEmail = "2"; // a driver's id
            _viewModel.SearchCommand.Execute(null);
            Assert.True(_viewModel.Customers.IsNullOrEmpty());
            return Task.CompletedTask;
    }
}