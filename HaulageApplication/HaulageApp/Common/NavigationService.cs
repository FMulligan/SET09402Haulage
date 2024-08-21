namespace HaulageApp.Common;

public class NavigationService: INavigationService
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }
}