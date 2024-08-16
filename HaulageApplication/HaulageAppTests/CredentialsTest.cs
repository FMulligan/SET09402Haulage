using HaulageApp.Data;
using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;
using HaulageApp.ViewModels;

namespace HaulageAppTests;

public class CredentialsTest
{
    public DbContextOptions CreateContextOptions()
    {
        var options = new DbContextOptionsBuilder<HaulageDbContext>()
            .UseInMemoryDatabase(databaseName: "MockDB")
            .Options;

        return options;
    }

    public void CreateContext(DbContextOptions options)
    {
        // Insert seed data into the database using one instance of the context
        using var context = new HaulageDbContext(options);
        context.AddRange(
            new User { Email = "customer", Password = "1234", Status = "active", Role = 1 },
            new User { Email = "admin", Password = "1234", Status = "inactive", Role = 3 }
        );
        context.SaveChanges();
    }
    
    [Fact]
    public void CredentialsReturnsTrueWhenDataExists()
    {
        var options = CreateContextOptions();
        
        CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new LoginViewModel(context);
            Assert.True(viewmodel.IsCredentialCorrect("customer", "1234"));
        }
    }
    
    [Fact]
    public void CredentialsReturnsFalseWhenDataDoesNotExist()
    {
        var options = CreateContextOptions();
        
        CreateContext(options);
        
        using (var context = new HaulageDbContext(options))
        {
            var viewmodel = new LoginViewModel(context);
            Assert.False(viewmodel.IsCredentialCorrect("who", "1234"));
        }
    }
}