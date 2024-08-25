using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageAppTests;

public class MockDb
{
    public DbContextOptions CreateContextOptions()
    {
        var options = new DbContextOptionsBuilder<HaulageDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //required because of how dotnet test runs the tests
            .Options;

        return options;
    }

    public void CreateContext(DbContextOptions options)
    {
        using var context = new HaulageDbContext(options);
        
        //role table
        var customerRole = new Role { Type = "customer"};
        var driverRole = new Role { Type = "driver" };
        var adminRole = new Role { Type = "admin" };
        context.role.Add(customerRole);
        context.role.Add(driverRole);
        context.role.Add(adminRole);

        context.SaveChanges();
        
        //user table
        context.user.Add(new User { Email = "customer", Password = "1234", Status = "active", Role = customerRole.Id });
        context.user.Add(new User { Email = "driver", Password = "1234", Status = "inactive", Role = driverRole.Id });
        context.user.Add(new User { Email = "admin", Password = "1234", Status = "active", Role = adminRole.Id });
        context.user.Add(new User { Email = "customer2", Password = "1234", Status = "active", Role = customerRole.Id });
        context.user.Add(new User { Email = "customer3", Password = "1234", Status = "active", Role = customerRole.Id });
        
        context.SaveChanges();
    }
}