using HaulageApp.Data;
using Microsoft.Maui.Storage;

namespace HaulageApp.Services
{
    public class UserService : IUserService
    {
        private readonly HaulageDbContext _context;

        public UserService(HaulageDbContext context)
        {
            _context = context;
        }

        public bool IsDriver()
        {
            var currentUserId = GetCurrentUserId();
            var user = _context.user.FirstOrDefault(u => u.Id == currentUserId);
            return user != null && user.Role == 2; 
        }

        public int GetCurrentUserId()
        {
            return Preferences.Get("currentUserId", 0);
        }
    }
}