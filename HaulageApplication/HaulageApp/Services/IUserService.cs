namespace HaulageApp.Services
{
    public interface IUserService
    {
        bool IsDriver();
        int GetCurrentUserId();
    }
}