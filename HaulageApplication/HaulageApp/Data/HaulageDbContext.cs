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

        public DbSet<Note> Notes { get; set; }
    }
}