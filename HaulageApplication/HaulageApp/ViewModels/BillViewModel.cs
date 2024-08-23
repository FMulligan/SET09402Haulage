using HaulageApp.Models;
using HaulageApp.Data;
using System.Collections.ObjectModel;

namespace HaulageApp.ViewModels
{
    public class BillViewModel
    {
        public Bill Bill { get; }

        public int BillId => Bill.BillId;
        public decimal Amount => Bill.Amount;
        public string Status => Bill.Status;
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        public BillViewModel(HaulageDbContext context, Bill bill)
        {
            Bill = bill;

            if (Bill.Items != null)
            {
                foreach (var item in Bill.Items)
                {
                    Items.Add(item);
                }
            }
        }
    }
}