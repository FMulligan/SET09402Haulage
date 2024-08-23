using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("vehicle")]
[PrimaryKey(nameof(Id))]
public class Vehicle
{
    public int Id { get; set; }
    
    [Required]
    public string Type { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }
    
    [Required]
    public string Status { get; set; }
    
}