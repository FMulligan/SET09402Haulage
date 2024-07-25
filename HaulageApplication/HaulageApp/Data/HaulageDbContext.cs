using Microsoft.EntityFrameworkCore;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.Data
{
    public class HaulageDbContext: DbContext
    {
        public HaulageDbContext()
        { }
        public HaulageDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Note> note { get; set; }
    }
}