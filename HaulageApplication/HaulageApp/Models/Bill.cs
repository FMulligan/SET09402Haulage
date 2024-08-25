using System.ComponentModel.DataAnnotations.Schema;
using HaulageApp.Models;

public class Bill
{
    [Column("bill_id")]
    public int BillId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>(); // Ensure Items is never null
}