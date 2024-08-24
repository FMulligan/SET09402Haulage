using HaulageApp.Models;
public class BillViewModelTests
{
    [Fact]
    public void BillViewModel_Constructor_SetsPropertiesCorrectly()
    {
        var bill = SharedTestSetup.CreateBillWithItems(1, 150.75m, "paid", new List<Item>
        {
            new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
            new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
        });

        var viewModel = SharedTestSetup.CreateBillViewModel(bill);
        
        Assert.Equal(bill.BillId, viewModel.BillId);
        Assert.Equal(bill.Amount, viewModel.Amount);
        Assert.Equal(bill.Status, viewModel.Status);
        Assert.Equal(2, viewModel.Items.Count);
    }

    [Fact]
    public void BillViewModel_Constructor_InitializesItemsCollectionCorrectly()
    {
        var bill = SharedTestSetup.CreateBillWithItems(2, 200.00m, "pending", new List<Item>
        {
            new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
            new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" }
        });
        
        var viewModel = SharedTestSetup.CreateBillViewModel(bill);
        
        Assert.NotNull(viewModel.Items);
        Assert.Equal(2, viewModel.Items.Count);
    }
    
    [Fact]
    public void BillViewModel_Constructor_InitializesEmptyItemsCollectionCorrectly()
    {
        var bill = SharedTestSetup.CreateBillWithItems(3, 300.00m, "pending", new List<Item>());

        var viewModel = SharedTestSetup.CreateBillViewModel(bill);

        Assert.NotNull(viewModel.Items);
        Assert.Empty(viewModel.Items);
    }
    
    [Fact]
    public void BillViewModel_Constructor_HandlesMultipleItemsCorrectly()
    {
        var bill = SharedTestSetup.CreateBillWithItems(5, 500.00m, "pending", new List<Item>
        {
            new Item { ItemId = 1, PickupLocation = "Loc A", DeliveryLocation = "Loc B", Status = "delivered" },
            new Item { ItemId = 2, PickupLocation = "Loc C", DeliveryLocation = "Loc D", Status = "pending" },
            new Item { ItemId = 3, PickupLocation = "Loc E", DeliveryLocation = "Loc F", Status = "pending" }
        });

        var viewModel = SharedTestSetup.CreateBillViewModel(bill);

        Assert.Equal(3, viewModel.Items.Count);
        Assert.Equal("Loc A", viewModel.Items[0].PickupLocation);
        Assert.Equal("Loc C", viewModel.Items[1].PickupLocation);
        Assert.Equal("Loc E", viewModel.Items[2].PickupLocation);
    }

    [Fact]
    public void BillViewModel_Constructor_HandlesNullItemsCollectionCorrectly()
    {
        var bill = SharedTestSetup.CreateBillWithItems(4, 400.00m, "paid", null);

        var viewModel = SharedTestSetup.CreateBillViewModel(bill);

        Assert.NotNull(viewModel.Items); 
        Assert.Empty(viewModel.Items);
    }
}
