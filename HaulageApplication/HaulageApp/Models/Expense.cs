using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaulageApp.Models;

[Table("expense")]
[PrimaryKey(nameof(Id))]
public class Expense
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(Driver))]
    public int Driver { get; set; }
    
    [ForeignKey(nameof(Trip))]
    public int Trip { get; set; }
    
    public Decimal Amount { get; set; }
    public string Status { get; set; }
}