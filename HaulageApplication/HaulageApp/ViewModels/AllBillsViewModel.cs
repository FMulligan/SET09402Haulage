using System.Collections.ObjectModel;
using System.ComponentModel;
using HaulageApp.Services;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels
{
    public class AllBillsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<BillViewModel> _allBills;
        public ObservableCollection<BillViewModel> AllBills
        {
            get => _allBills;
            private set
            {
                if (_allBills != value)
                {
                    _allBills = value;
                    OnPropertyChanged(nameof(AllBills));
                }
            }
        }

        private readonly IBillService _billService;
        private readonly IUserService _userService;
        private readonly ILogger<AllBillsViewModel> _logger;

        public AllBillsViewModel(IBillService billService, IUserService userService, ILogger<AllBillsViewModel> logger)
        {
            _billService = billService;
            _userService = userService;
            _logger = logger;
        }

        public async Task LoadBillsAsync()
        {
            int currentUserId = _userService.GetCurrentUserId();

            try
            {
                if (currentUserId == 0)
                {
                    _logger.LogWarning("No current user ID found in Preferences.");
                    AllBills = new ObservableCollection<BillViewModel>();
                    return;
                }
                var bills = await _billService.GetBillsForCurrentUserAsync(currentUserId);
                AllBills = new ObservableCollection<BillViewModel>(
                    bills.Select(b => new BillViewModel(b)).ToList()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing AllBills.");
                AllBills = new ObservableCollection<BillViewModel>();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
