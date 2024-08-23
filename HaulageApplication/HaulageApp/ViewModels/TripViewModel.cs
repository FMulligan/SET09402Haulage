using HaulageApp.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels
{
    public class TripViewModel : INotifyPropertyChanged
    {
        private readonly HaulageDbContext _context;
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

        public TripViewModel(HaulageDbContext context)
        {
            _context = context;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var trips = await _context.trip
                    .Include(t => t.Waypoints)
                    .ToListAsync();

                TripGroups = new ObservableCollection<TripItemViewModel>(
                    trips.Select(trip => new TripItemViewModel(trip, _context))
                );
            }
            catch (Exception ex)
            {
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