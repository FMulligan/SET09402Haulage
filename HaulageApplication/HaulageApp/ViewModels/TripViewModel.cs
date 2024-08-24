using System.Collections.ObjectModel;
using HaulageApp.Data;
using HaulageApp.Services;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels
{
    public class TripViewModel : INotifyPropertyChanged
    {
        private readonly HaulageDbContext _context;
        private readonly IUserService _userService;
        private ObservableCollection<TripItemViewModel> _tripGroups;

        public ObservableCollection<TripItemViewModel> TripGroups
        {
            get => _tripGroups;
            set
            {
                _tripGroups = value;
                OnPropertyChanged(nameof(TripGroups));
            }
        }

        public TripViewModel(HaulageDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                bool canEdit = _userService.IsDriver();

                var trips = await _context.trip
                    .Include(t => t.Waypoints)
                    .ToListAsync();

                TripGroups = new ObservableCollection<TripItemViewModel>(
                    trips.Select(trip => new TripItemViewModel(trip, _context, canEdit))
                );
            }
            catch (Exception ex)
            {
                TripGroups = new ObservableCollection<TripItemViewModel>(); 
                Console.WriteLine($"Error loading data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}