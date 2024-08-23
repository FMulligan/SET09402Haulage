using HaulageApp.Data;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class PermissionsModelTests
{
    private readonly MockDb _db = new();
    private FakePreferencesWrapper fakeStorage = new();
    
    [Fact]
    public void AssertPageVisibilityWhenUserIsCustomer()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            PermissionsViewModel permissionsViewModel = new PermissionsViewModel(context, fakeStorage);

            fakeStorage.Set<string>("hasAuth", "customer");
            permissionsViewModel.UpdateTabsForCurrentUser();
            Assert.False(permissionsViewModel.ManageCustomersIsVisible);
            Assert.False(permissionsViewModel.NotesIsVisible);
        }
    }
    
    [Fact]
    public void AssertPageVisibilityWhenUserIsDriver()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            PermissionsViewModel permissionsViewModel = new PermissionsViewModel(context, fakeStorage);

            fakeStorage.Set<string>("hasAuth", "driver");
            permissionsViewModel.UpdateTabsForCurrentUser();
            Assert.False(permissionsViewModel.ManageCustomersIsVisible);
            Assert.True(permissionsViewModel.NotesIsVisible);
        }
    }
    
    [Fact]
    public void AssertPageVisibilityWhenUserIsAdmin()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            PermissionsViewModel permissionsViewModel = new PermissionsViewModel(context, fakeStorage);

            fakeStorage.Set<string>("hasAuth", "admin");
            permissionsViewModel.UpdateTabsForCurrentUser();
            Assert.True(permissionsViewModel.ManageCustomersIsVisible);
            Assert.False(permissionsViewModel.NotesIsVisible);
        }
    }
}