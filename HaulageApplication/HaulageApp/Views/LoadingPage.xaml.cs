namespace HaulageApp.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();

    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (IsAuthenticated())
        {
            await Shell.Current.GoToAsync("///home");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
    }

    private bool IsAuthenticated()
    {
        var hasAuth = Preferences.Default.ContainsKey("hasAuth");
        return hasAuth;
    }
}