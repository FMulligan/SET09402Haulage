using Microsoft.Maui.Storage;

namespace HaulageApp.Services
{
    public class UserService : IUserService
    {
        public int GetCurrentUserId()
        {
            return Preferences.Get("currentUserId", 0);
        }
    }
}