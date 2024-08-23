using HaulageApp.Models;
using HaulageApp.ViewModels;
using System.Collections.Generic;
using Xunit;

public class BillViewModelTests
{
    [Fact]
    public void BillViewModel_Constructor_SetsPropertiesCorrectly()
    {
        var bill = new Bill
        {
            BillId = 1,
            Amount = 150.75m,
            Status = "paid",
            Items = new List<Item>
            {
                new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
                new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
            }
        };

        var viewModel = new BillViewModel(null, bill);
        
        Assert.Equal(bill.BillId, viewModel.BillId);
        Assert.Equal(bill.Amount, viewModel.Amount);
        Assert.Equal(bill.Status, viewModel.Status);
        Assert.Equal(2, viewModel.Items.Count);
    }

    [Fact]
    public void BillViewModel_Constructor_InitializesItemsCollectionCorrectly()
    {
        var bill = new Bill
        {
            BillId = 2,
            Amount = 200.00m,
            Status = "pending",
            Items = new List<Item>
            {
                new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
                new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
            }
        };
        
        var viewModel = new BillViewModel(null, bill);
        
        Assert.NotNull(viewModel.Items);
        Assert.Equal(2, viewModel.Items.Count);
    }

    [Fact]
    public void BillViewModel_Constructor_SetsEmptyItemsCollection_WhenItemsIsNull()
    {
        var bill = new Bill
        {
            BillId = 3,
            Amount = 250.00m,
            Status = "paid",
            Items = null 
        };
        
        var viewModel = new BillViewModel(null, bill);
        
        Assert.Empty(viewModel.Items);
    }
}
