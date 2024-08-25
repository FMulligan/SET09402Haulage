namespace HaulageApp.Services;

public class NavigationService: INavigationService
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync(string route, Dictionary<string, object>? dictionary)
    {
        await Shell.Current.GoToAsync(route, dictionary);
    }
}