using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageAppTests;

public class MockDb
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
        //user table
        context.user.Add(new User { Email = "customer", Password = "1234", Status = "active", Role = 1 });
        context.user.Add(new User { Email = "driver", Password = "1234", Status = "inactive", Role = 2 });
        context.user.Add(new User { Email = "admin", Password = "1234", Status = "active", Role = 3 });
        context.SaveChanges();
    }
}