using SQLite;

namespace HaulageApp.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }
}