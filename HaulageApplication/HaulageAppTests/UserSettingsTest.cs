using HaulageApp.Data;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class UserSettingsTest
{
    private MockDb db = new();
    private FakeSecureStorageWrapper fakeStorage = new();

    [Fact]
    public async void ReturnsUserEmailWhenAuthenticated()
    {
        var options = db.CreateContextOptions();
        db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            await fakeStorage.SetAsync("hasAuth", "testEmail");
            var viewmodel = new SettingsViewModel(context, fakeStorage);
            Assert.Equal("testEmail", viewmodel.Email);
        }
    }

    [Fact]
    public void ReturnsErrorMessageWhenNotAuthenticated()
    {
        var options = db.CreateContextOptions();
        db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new SettingsViewModel(context, fakeStorage);
            Assert.Equal("Error retrieving email.", viewmodel.Email);
        }
    }

    [Fact]
    public void ReturnsTrueForValidPassword()
    {
        var options = db.CreateContextOptions();
        db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new SettingsViewModel(context, fakeStorage);
            Assert.True(viewmodel.PasswordIsValid("123456"));
        }
    }
    
    [Fact]
    public void ReturnsFalseForInvalidPassword()
    {
        var options = db.CreateContextOptions();
        db.CreateContext(options);

        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new SettingsViewModel(context, fakeStorage);
            //empty
            Assert.False(viewmodel.PasswordIsValid(""));
            //potential injection
            Assert.False(viewmodel.PasswordIsValid("10; DROP TABLE members /*"));
            //too short
            Assert.False(viewmodel.PasswordIsValid("1234"));
        }
    }
}