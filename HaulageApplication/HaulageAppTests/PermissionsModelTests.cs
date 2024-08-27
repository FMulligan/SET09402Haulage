using HaulageApp.Data;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class PermissionsModelTests
{
    private readonly MockDb _db = new();
    private FakePreferencesWrapper fakeStorage = new();
    private readonly HaulageDbContext _context;
    private PermissionsViewModel _viewModel;

    public PermissionsModelTests()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);
        _context = new HaulageDbContext(options);
        _viewModel = new PermissionsViewModel(_context, fakeStorage);
    }

    [Fact]
    public void AssertPageVisibilityWhenUserIsCustomer()
    {
        fakeStorage.Set<string>("hasAuth", "customer");
        _viewModel.UpdateTabsForCurrentUser();
        Assert.False(_viewModel.ManageCustomersIsVisible);
        Assert.True(_viewModel.BillsIsVisible);
        Assert.False(_viewModel.TripsIsVisible);
    }

    [Fact]
    public void AssertPageVisibilityWhenUserIsDriver()
    {
        fakeStorage.Set<string>("hasAuth", "driver");
        _viewModel.UpdateTabsForCurrentUser();
        Assert.False(_viewModel.ManageCustomersIsVisible);
        Assert.False(_viewModel.BillsIsVisible);
        Assert.True(_viewModel.TripsIsVisible);
    }

    [Fact]
    public void AssertPageVisibilityWhenUserIsAdmin()
    {
        fakeStorage.Set<string>("hasAuth", "admin");
        _viewModel.UpdateTabsForCurrentUser();
        Assert.True(_viewModel.ManageCustomersIsVisible);
        Assert.False(_viewModel.BillsIsVisible);
        Assert.True(_viewModel.TripsIsVisible);
    }
}