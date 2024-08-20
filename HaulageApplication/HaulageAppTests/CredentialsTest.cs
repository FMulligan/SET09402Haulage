using HaulageApp.Data;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class CredentialsTest
{
    private MockDb db = new();
    
    [Fact]
    public void CredentialsReturnsTrueWhenDataExists()
    {
        var options = db.CreateContextOptions();
        
        db.CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new LoginViewModel(context);
            Assert.True(viewmodel.IsCredentialCorrect("customer", "1234"));
        }
    }
    
    [Fact]
    public void CredentialsReturnsFalseWhenDataDoesNotExist()
    {
        var options = db.CreateContextOptions();
        
        db.CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new LoginViewModel(context);
            Assert.False(viewmodel.IsCredentialCorrect("who", "1234"));
        }
    }
}