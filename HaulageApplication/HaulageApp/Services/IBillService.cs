using System.Collections.Generic;
using System.Threading.Tasks;
using HaulageApp.Models;

namespace HaulageApp.Services
{
    public interface IBillService
    {
        Task<List<Bill>> GetBillsForCurrentUserAsync(int userId);
    }
}