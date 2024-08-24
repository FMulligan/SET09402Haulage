// SharedTestSetup.cs
using HaulageApp.Models;
using System.Collections.Generic;
using HaulageApp.ViewModels;

public static class SharedTestSetup
{
    public static Bill CreateBillWithItems(int billId, decimal amount, string status, List<Item> items = null)
    {
        return new Bill
        {
            BillId = billId,
            Amount = amount,
            Status = status,
            Items = items ?? new List<Item>()
        };
    }

    public static BillViewModel CreateBillViewModel(Bill bill)
    {
        return new BillViewModel(bill);
    }
}