using System.Collections.ObjectModel;
using HaulageApp.Models;

namespace HaulageApp.ViewModels
{
    public class BillViewModel
    {
        public Bill Bill { get; }

        public int BillId => Bill.BillId;
        public decimal Amount => Bill.Amount;
        public string Status => Bill.Status;
        public ObservableCollection<Item> Items { get; }

        public BillViewModel(Bill bill)
        {
            Bill = bill;
            Items = new ObservableCollection<Item>(bill.Items);
        }
    }
}