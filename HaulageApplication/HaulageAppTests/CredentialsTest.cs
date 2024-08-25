using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class CredentialsTest
{
    private readonly MockDb _db = new();
    private FakePreferencesWrapper fakeStorage = new();
    
    [Fact]
    public void CredentialsReturnsTrueWhenDataExists()
    {
        var options = _db.CreateContextOptions();
        _db.CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var permissionsViewModel = new PermissionsViewModel(context, fakeStorage);
            var viewmodel = new LoginViewModel(context, permissionsViewModel);
            User? user = context.user.FirstOrDefault();
            Assert.True(viewmodel.IsCredentialCorrect(user!, "1234"));
        }
    }
    
    [Fact]
    public void CredentialsReturnsFalseWhenDataDoesNotExist()
    {
        var options = _db.CreateContextOptions();
        
        _db.CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var permissionsViewModel = new PermissionsViewModel(context, fakeStorage);
            var viewmodel = new LoginViewModel(context, permissionsViewModel);
            User? user = context.user.FirstOrDefault();
            Assert.False(viewmodel.IsCredentialCorrect(user!, "3456"));
        }
    }
}