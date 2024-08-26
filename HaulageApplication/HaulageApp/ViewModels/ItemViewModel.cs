using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels;

public partial class ItemViewModel: ObservableObject, IQueryAttributable
{

    private readonly HaulageDbContext _context;
    private readonly ILogger<ItemViewModel> _logger;
    private Item? _item;
    private int _billId;
    private int _tripId;

    public ItemViewModel(HaulageDbContext context, ILogger<ItemViewModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("billId", out var value))
        {
            _billId = value as int? ?? 0;
        }
        if (query.TryGetValue("tripId", out var value2))
        {
            _tripId = (int)value2;
        }
        if (query.TryGetValue("item", out var value3))
        {
            _item = value3 as Item;
            if (_item != null)
            {
                PickupLocation = _item.PickupLocation;
                DeliveryLocation = _item.DeliveryLocation;
                Status = _item.Status;
            }
        }
    }

    private String _pickupLocation;
    public String PickupLocation
    {
        get => _pickupLocation;
        set
        {
            if (_pickupLocation != value)
            {
                _pickupLocation = value;
                OnPropertyChanged();
            }
        }
    }

    private String _deliveryLocation;
    public String DeliveryLocation
    {
        get => _deliveryLocation;
        set
        {
            if (_deliveryLocation != value)
            {
                _deliveryLocation = value;
                OnPropertyChanged();
            }
        }
    }

    private String _status;
    public String Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    [RelayCommand]
    public async Task Save()
    {
        //Check if PickupLocation, DeliveryLocation or Status is null
        if (string.IsNullOrWhiteSpace(PickupLocation) || string.IsNullOrWhiteSpace(DeliveryLocation) || string.IsNullOrWhiteSpace(Status))
        {
            await Shell.Current.DisplayAlert("Error", "Invalid Input, feilds must be specified " +
                                                      "\n- Pickup Location must not be null \n- Delivery Location must not be null " +
                                                      "\n- Status must not be null", "Confirm");
            return;
        }
        // Check if the status is not pending if the Pickup/Delivery locations have been changed
        if (Status != "pending" && (_item == null || PickupLocation != _item.PickupLocation || DeliveryLocation != _item.DeliveryLocation))
        {
            // Show error message and stop saving
            await Shell.Current.DisplayAlert("Error", "Cannot edit Pickup/Delivery Location when status is not pending.", "Confirm");
            return;
        }
        try
        {
            if (_item == null)
            {
                //add a new item
                _item = new Item
                {
                    BillId = _billId,
                    TripId = _tripId,
                    PickupLocation = PickupLocation,
                    DeliveryLocation = DeliveryLocation,
                    Status = Status
                };
                _context.item.Add(_item);
                await _context.SaveChangesAsync();
            }
            else //update an existing item
            {
                try
                {
                    await _context.item
                        .AsQueryable()
                        .Where(i => i.ItemId == _item.ItemId)
                        .ExecuteUpdateAsync(x => x
                            .SetProperty(i => i.PickupLocation, i => PickupLocation)
                            .SetProperty(i => i.DeliveryLocation, i => DeliveryLocation)
                            .SetProperty(i => i.Status, i => Status)
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error Saving changes to Item");
                }
            }

            await _context.Entry(_item).ReloadAsync();
            await Shell.Current.GoToAsync($"..?saved={_item.ItemId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Saving Item");
            await Shell.Current.DisplayAlert("Error", "Failed to save item", "Confirm");
        }
    }

    [RelayCommand]
    public async Task Delete()
    {
        //Cannot delete a new item
        if (_item == null)
        {
            await Shell.Current.DisplayAlert("Error", "Cannot delete a new item", "Confirm");
            return;
        }
        //Cannot delete item if not status pending
        if (Status != "pending")
        {
            await Shell.Current.DisplayAlert("Error", "Cannot delete item if picked up or delivered", "Confirm");
            return;
        }

        try
        {
            _context.item.Remove(_item);
            await _context.SaveChangesAsync();
            await Shell.Current.GoToAsync($"..?deleted={_item.ItemId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Deleting Item");
            await Shell.Current.DisplayAlert("Error", "Failed to delete item", "Confirm");
        }
    }

}