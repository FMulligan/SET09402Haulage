using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace HaulageAppTests;

public class ItemTests
{
    private readonly Mock<DbSet<Item>> _mockItems;
    private readonly Mock<HaulageDbContext> _mockContext;
    private readonly Mock<ILogger<ItemViewModel>> _loggerMock;
    private readonly ItemViewModel _viewModel;
    private readonly Bill _bill;
    private readonly Trip _trip;
    private readonly Item _item;

    public ItemTests()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockItems = new Mock<DbSet<Item>>();
        _loggerMock = new Mock<ILogger<ItemViewModel>>();
        _viewModel = new ItemViewModel(_mockContext.Object, _loggerMock.Object);

        _bill = new Bill
        {
            BillId = 1, CustomerId = 1, Amount = 100.20m, Status = "pending", Items = new List<Item>
            {
                new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
                new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
            }
        };

        _trip = new Trip { Id = 1 };

        _item = new Item
        {
            ItemId = 3, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending"
        };

        _mockContext.Setup(e => e.item).Returns(_mockItems.Object);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldAssignItemProperties()
    {
        var query = new Dictionary<string, object> { { "billId", _bill.BillId }, { "item", _item } };
        _viewModel.ApplyQueryAttributes(query);

        Assert.Equal("Loc C", _viewModel.PickupLocation);
        Assert.Equal("Loc D", _viewModel.DeliveryLocation);
        Assert.Equal("pending", _viewModel.Status);
    }

    [Fact]
    public Task SaveShouldAddNewItemWhenItemIsNull()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "billId", 1 },{ "tripId", 1 }});

        _viewModel.PickupLocation = "LocA";
        _viewModel.DeliveryLocation = "LocB";
        _viewModel.Status = "pending";

        _viewModel.Save();

        _mockItems.Verify(c => c.Add(It.Is<Item>(i => i.DeliveryLocation == "LocB")), Times.Once); 
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        return Task.CompletedTask;

    }
    [Fact]
    public Task SaveShouldUpdateEventIfExisting()
    {
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "item", _item } });
        _viewModel.PickupLocation = "LocC";
        _viewModel.DeliveryLocation = "LocD";
        _viewModel.Status = "pending";

        _mockContext.Setup(e => e.Entry(_item)).Returns(new Mock<FakeEntityEntry<Item>>().Object);
        _viewModel.Save();

        _mockItems.Verify(c => c.AsQueryable(), Times.Once);
        _mockItems.Verify(c => c.Add(It.IsAny<Item>()), Times.Never);
        return Task.CompletedTask;

    }

    [Fact] public Task DeleteShouldRemoveEvent() 
    { 
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "item", _item } });

        _viewModel.Delete();

        _mockItems.Verify(c => c.Remove(It.IsAny<Item>()), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        return Task.CompletedTask;
    }
}