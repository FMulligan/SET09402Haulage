namespace HaulageApp.Common;

public interface INavigationService
{
    Task GoToAsync(string route);
}