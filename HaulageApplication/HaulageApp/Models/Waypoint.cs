using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models
{
    [Table("waypoint")]  
    public class Waypoint
    {
        [Key]
        [Column("waypoint_id")]  
        public int WaypointId { get; set; }

        [ForeignKey("trip")]
        [Column("trip_id")] 
        public int TripId { get; set; }

        [ForeignKey("user")]
        [Column("user_id")] 
        public int UserId { get; set; }

        [Column("location")]  
        public string Location { get; set; }

        [Column("arrival_time")]  
        public DateTime? ArrivalTime { get; set; }

        [Column("departure_time")]  
        public DateTime? DepartureTime { get; set; }

        [Column("waypoint_type")]  
        public string WaypointType { get; set; }


        public virtual Trip Trip { get; set; }
    }
}