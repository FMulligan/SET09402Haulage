using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.Models
{
    [Table("trip")]
    [PrimaryKey(nameof(Id))]
    public class Trip
    {
        public int Id { get; set; }
        
        [ForeignKey(nameof(Vehicle))]
        public int Vehicle { get; set; }
        [ForeignKey(nameof(Driver))]
        public int Driver { get; set; } 

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; } 

        [Column("status")]
        public string Status { get; set; } 

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } 
        public virtual ICollection<Waypoint> Waypoints { get; set; }
    }
}