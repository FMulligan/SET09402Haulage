namespace HaulageApp.Services;

public interface INavigationService
{
    Task GoToAsync(string route);
    
    Task GoToAsync(string route, Dictionary<string, object>? dictionary);

}