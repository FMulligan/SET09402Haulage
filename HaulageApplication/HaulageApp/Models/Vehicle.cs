using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("vehicle")]
[PrimaryKey(nameof(Id))]
public class Vehicle
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public string Status { get; set; }
    
}