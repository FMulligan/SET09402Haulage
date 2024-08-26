using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageAppTests;

public class MockDb
{
    public DbContextOptions<HaulageDbContext> CreateContextOptions()
    {
        var options = new DbContextOptionsBuilder<HaulageDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //required because of how dotnet test runs the tests
            .Options;

        return options;
    }

    public void CreateContext(DbContextOptions<HaulageDbContext> options)
    {
        using var context = new HaulageDbContext(options);
        //role table
        var customerRole = new Role { Type = "customer"};
        var driverRole = new Role { Type = "driver" };
        var adminRole = new Role { Type = "admin" };
        context.role.Add(customerRole);
        context.role.Add(driverRole);
        context.role.Add(adminRole);
        
        context.user.Add(new User { Email = "customer", Password = "1234", Status = "active", Role = customerRole.Id });
        context.user.Add(new User { Email = "driver", Password = "1234", Status = "inactive", Role = driverRole.Id });
        context.user.Add(new User { Email = "admin", Password = "1234", Status = "active", Role = adminRole.Id });
        context.user.Add(new User { Email = "customer2", Password = "1234", Status = "active", Role = customerRole.Id });
        context.user.Add(new User { Email = "customer3", Password = "1234", Status = "active", Role = customerRole.Id });
        context.SaveChanges();
    }

    public void CreateContextWithTrips(DbContextOptions<HaulageDbContext> options)
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

        context.trip.AddRange(
            new Trip { Id = 1, StartTime = DateTime.Now.AddHours(-2), Status = "ongoing" },
            new Trip { Id = 2, StartTime = DateTime.Now.AddHours(-4), Status = "completed" }
        );
        
        context.bill.AddRange(
            new Bill { BillId = 1, CustomerId = 1, Amount = 100.20m, Status = "pending", Items = new List<Item>
            {
                new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
                new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
            }},
            new Bill { BillId = 2, CustomerId = 4, Amount = 200.00m, Status = "pending", Items = new List<Item>
            {
                new Item { ItemId = 3, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
                new Item { ItemId = 4, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
            } }
        );

        context.SaveChanges();
    }

    public void ClearDatabase(DbContextOptions<HaulageDbContext> options)
    {
        using var context = new HaulageDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}