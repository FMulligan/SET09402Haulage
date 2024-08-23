using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageApp.ViewModels;

public partial class VehicleViewModel : ObservableObject, IQueryAttributable, INotifyDataErrorInfo
{
    private readonly HaulageDbContext _context;
    private Vehicle _vehicle;
    private Dictionary<string, List<string>> _errors = new();

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

    public bool HasErrors => _errors.Count != 0;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string property)
    {
        return _errors.ContainsKey(property) ? _errors[property] : Enumerable.Empty<string>();
    }

    public string Type
    {
        get => _vehicle.Type;
        set
        {
            if (_vehicle.Type != value)
            {
                _vehicle.Type = value;
                ValidateProperty(value, nameof(Type));
                OnPropertyChanged();
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
                ValidateProperty(value, nameof(Capacity));
                OnPropertyChanged();
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
                ValidateProperty(value, nameof(Status));
                OnPropertyChanged();
            }
        }
    }

    public int Id => _vehicle.Id;
    
    [RelayCommand]
    private async Task Save()
    {
        ValidateProperty(Type, nameof(Type));
        ValidateProperty(Capacity, nameof(Capacity));
        ValidateProperty(Status, nameof(Status));

        if (HasErrors)
        {
            await Shell.Current.DisplayAlert("Error", "Please use valid values for fields \n -Type is required \n -Capacity must be a positive number \n -Status is required", "Confirm");
            return;
        }
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
        _context.vehicle.Remove(_vehicle);
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

    private void ValidateProperty(object value, string property)
    {
        var validContext = new ValidationContext(_vehicle) { MemberName = property };
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateProperty(value, validContext, validationResults);

        if (validationResults.Count > 0)
        {
            _errors[property] = validationResults.Select(c => c.ErrorMessage).ToList();
        }
        else
        {
            _errors.Remove(property);
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
    }

}