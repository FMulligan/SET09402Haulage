using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();

    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await IsAuthenticated())
        {
            await Shell.Current.GoToAsync("///home");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
    }

    async Task<bool> IsAuthenticated()
    {
        var hasAuth = await SecureStorage.GetAsync("hasAuth");
        return hasAuth != null;
    }
}