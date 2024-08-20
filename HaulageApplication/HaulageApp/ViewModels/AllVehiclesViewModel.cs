using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;

namespace HaulageApp.ViewModels;

public class AllVehiclesViewModel: IQueryAttributable
{
    public ObservableCollection<VehicleViewModel> AllVehicles { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectVehicleCommand { get; }

    private readonly HaulageDbContext _context;
    public AllVehiclesViewModel(HaulageDbContext vehicleContext)
        {
            _context = vehicleContext;

            try
            {
                var vehicles = _context.vehicle.ToList().Select(n => new VehicleViewModel(_context, n));
                AllVehicles = new ObservableCollection<VehicleViewModel>(vehicles);
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error","Error initialising AllVehicles", "Confirm");
                AllVehicles = new ObservableCollection<VehicleViewModel>(); // Initialize to avoid null reference
            }

            NewCommand = new AsyncRelayCommand(NewVehicleAsync);
            SelectVehicleCommand = new AsyncRelayCommand<VehicleViewModel>(SelectVehicleAsync);
        }
        private async Task NewVehicleAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.VehiclePage));
        }

        private async Task SelectVehicleAsync(VehicleViewModel? vehicle)
        {
            if (vehicle != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.VehiclePage)}?load={vehicle.Id}");
            }
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string vehicleIdStr = query["deleted"].ToString();
                if (int.TryParse(vehicleIdStr, out int vehicleId))
                {
                    var matchedVehicle = AllVehicles.FirstOrDefault(n => n.Id == vehicleId);

                    // If vehicle exists, delete it
                    if (matchedVehicle != null)
                    {
                        AllVehicles.Remove(matchedVehicle);
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Warning",$"Vehicle not found in AllVehicles", "Confirm");
                    }
                }
                else
                {
                    Shell.Current.DisplayAlert("Error",$"Invalid Vehicle in query for deletion", "Confirm");
                }
            }
            else if (query.ContainsKey("saved"))
            {
                string vehicleIdStr = query["saved"].ToString();
                if (int.TryParse(vehicleIdStr, out int vehicleId))
                {
                    var matchedVehicle = AllVehicles.FirstOrDefault(n => n.Id == vehicleId);

                    // If vehicle is found, update it
                    if (matchedVehicle != null)
                    {
                        matchedVehicle.Reload();
                        AllVehicles.Move(AllVehicles.IndexOf(matchedVehicle), 0);
                    }
                    // If vehicle isn't found, it's new; add it.
                    else
                    {
                        var newVehicle = new VehicleViewModel(_context, _context.vehicle.Single(n => n.Id == vehicleId));
                        AllVehicles.Insert(0, newVehicle);
                    }
                }
                else
                {
                    Shell.Current.DisplayAlert("Error",$"Invalid Vehicle in query for saving", "Confirm");
                }
            }
        }
        
}
