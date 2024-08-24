using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("item")]
public class Item
{
    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("trip_id")]
    public int TripId { get; set; }

    [Column("bill_id")]
    public int BillId { get; set; }

    [Column("pickup_location")]
    public string PickupLocation { get; set; }

    [Column("delivery_location")]
    public string DeliveryLocation { get; set; }

    [Column("status")]
    public string Status { get; set; }
}