using System.Collections.ObjectModel;
using HaulageApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels
{
    public class AllBillsViewModel
    {
        public ObservableCollection<BillViewModel> AllBills { get; private set; }

        private readonly HaulageDbContext _context;
        private readonly ILogger<AllBillsViewModel> _logger;

        public AllBillsViewModel(HaulageDbContext context, ILogger<AllBillsViewModel> logger)
        {
            _context = context;
            _logger = logger;

            int currentUserId = GetCurrentUserId();

            try
            {
                if (currentUserId == 0)
                {
                    AllBills = new ObservableCollection<BillViewModel>(); 
                    return;
                }

                var bills = _context.bill
                    .Include(b => b.Items)
                    .Where(b => b.CustomerId == currentUserId)  
                    .ToList();

                AllBills = new ObservableCollection<BillViewModel>(
                    bills.Select(b => new BillViewModel(_context, b))
                );
            }
            catch (Exception ex)
            {
                AllBills = new ObservableCollection<BillViewModel>();
            }
        }

        protected virtual int GetCurrentUserId()
        {
            return Preferences.Get("currentUserId", 0); 
        }
    }
}
