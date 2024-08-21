using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;

namespace HaulageApp.Data
{
    public class HaulageDbContext: DbContext
    {
        public HaulageDbContext()
        { }
        public HaulageDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Note> note { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Trip> trip { get; set; }
        public DbSet<Vehicle> vehicle { get; set; }
        public virtual DbSet<Expense> expense { get; set; }
    }
}