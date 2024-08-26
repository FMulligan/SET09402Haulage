using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels;

public partial class AllItemsViewModel: ObservableObject
{

    private readonly HaulageDbContext _context;
    public ObservableCollection<Item> AllItems { get;  } = []; 

    private ObservableCollection<int> _billIds;
    public ObservableCollection<int> BillIds
    {
        get => _billIds;
        private set
        {
            if (_billIds != value)
            {
                _billIds = value;
                OnPropertyChanged();
            }
        }
    }

    private readonly IUserService _userService;
    private readonly IBillService _billService;
    private readonly ILogger<AllItemsViewModel> _logger;

    public AllItemsViewModel(HaulageDbContext context, IUserService userService, IBillService billService, ILogger<AllItemsViewModel> logger)
    {
        _context = context;
        _userService = userService;
        _billService = billService;
        _logger = logger;
    }

    private Item _selectedItem;
    public Item SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
                // Automatically trigger the SelectItem command when the selection changes
                SelectItemCommand.Execute(_selectedItem);
            }
        }
    }

    public async Task LoadAllItemsForCustomer()
    {
        AllItems.Clear();
        int userId = _userService.GetCurrentUserId();
        try
        {
            if (userId == 0)
            {
                _logger.LogWarning("No User Id found for current user");
                BillIds = new ObservableCollection<int>();
                return;
            }

            var bills = await _billService.GetBillsForCurrentUserAsync(userId);
            BillIds = new ObservableCollection<int>(
                bills.Select(b => b.BillId).ToList());

            LoadIdsForItems();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Loading all items for customer");
        }
    }

    private async void LoadIdsForItems()
    {
        var items = await _context.item
            .AsQueryable()
            .Where(i => BillIds.Contains(i.BillId))
            .ToListAsync();

        foreach (var i in items)
        {
            AllItems.Add(i);
        }
    }

    public int GetLatestTripId()
    {
        var tripId = _context.trip
            .AsQueryable()
            .OrderByDescending(i => i.Id)
            .FirstOrDefault()!.Id;
        return tripId == null ? 0 : tripId;
    }

    [RelayCommand]
    private async Task AddItem()
    {
        await Shell.Current.GoToAsync(nameof(Views.ItemPage), new Dictionary<string, object>{{"billId", _billIds.LastOrDefault()},{"tripId", GetLatestTripId()}});
    }

    [RelayCommand]
    private async Task SelectItem()
    {
        await Shell.Current.GoToAsync(nameof(Views.ItemPage), 
            new Dictionary<string, object>{{"billId", _billIds.LastOrDefault()},{"tripId", GetLatestTripId()}, {"item", SelectedItem}});
    }
}