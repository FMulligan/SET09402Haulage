using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("event")]
public class Event
{
    [Key]
    [Column("event_id")] 
    public int Id { get; set; }
    
    [ForeignKey(nameof(Trip))]
    [Column("trip_id")] 
    public int TripId { get; set; }
    
    [Column("event_type")]  
    public string EventType { get; set; }
    
    [Column("timestamp")]  
    public DateTime? Timestamp { get; set; }
    
}