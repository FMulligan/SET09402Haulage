using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageApp.ViewModels;

public partial class VehicleViewModel : ObservableObject, IQueryAttributable
{
    private readonly HaulageDbContext _context;
    private Vehicle _vehicle;

    public VehicleViewModel(HaulageDbContext dbContext)
    {
        _context = dbContext;
        _vehicle = new Vehicle();
    }
    
    public VehicleViewModel(HaulageDbContext dbContext, Vehicle vehicle)
    {
        _context = dbContext;
        _vehicle = vehicle;
    }

    public string Type
    {
        get => _vehicle.Type;
        set
        {
            if (_vehicle.Type != value)
            {
                _vehicle.Type = value;
            }
        }
    }
    
    public int Capacity
    {
        get => _vehicle.Capacity;
        set
        {
            if (_vehicle.Capacity != value)
            {
                _vehicle.Capacity = value;
            }
        }
    }
    
    public string Status
    {
        get => _vehicle.Status;
        set
        {
            if (_vehicle.Status != value)
            {
                _vehicle.Status = value;
            }
        }
    }

    public int Id => _vehicle.Id;
    
    [RelayCommand]
    private async Task Save()
    {
        if (_vehicle.Id == 0)
        {
            _context.vehicle.Add(_vehicle);
        }
        _context.SaveChanges();
        await Shell.Current.GoToAsync($"..?saved={_vehicle.Id}");
    }

    [RelayCommand]
    private async Task Delete()
    {
        _context.Remove(_vehicle);
        _context.SaveChanges();
        await Shell.Current.GoToAsync($"..?deleted={_vehicle.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        { 
            int vehicleId = int.Parse(query["load"].ToString());
            _vehicle = _context.vehicle.Single(n => n.Id == vehicleId);
            RefreshProperties();
        }
    }
    public void Reload()
    {
        _context.Entry(_vehicle).Reload();
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Type));
        OnPropertyChanged(nameof(Capacity));
        OnPropertyChanged(nameof(Status));
    }

}