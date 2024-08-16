using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("user")]
[PrimaryKey(nameof(Id))]
public class User
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(Role))]
    public int Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
}