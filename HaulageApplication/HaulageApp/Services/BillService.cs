using System.Collections.Generic;
using System.Threading.Tasks;
using HaulageApp.Data;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.Services
{
    public class BillService : IBillService
    {
        private readonly HaulageDbContext _context;

        public BillService(HaulageDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bill>> GetBillsForCurrentUserAsync(int userId)
        {
            return await _context.bill
                .Include(b => b.Items)
                .Where(b => b.CustomerId == userId)
                .ToListAsync();
        }
    }
}