using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("role")]
[PrimaryKey(nameof(Id))]
public class Role
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(Type))]
    public string Type { get; set; }
}