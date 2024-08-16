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
        // We would actually have a method to check authentication state.
        // E.g. would provide a token or, even, can save in local storage
        return false;
    }
}