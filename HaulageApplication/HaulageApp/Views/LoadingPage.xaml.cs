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
            // Should be a page that all roles have access to, e.g. settings.
            // Another option is to add the role to user defaults and can go 
            // to a different page based on which role is logged in. (e.g. switch
            // case based on role)
            await Shell.Current.GoToAsync("///settings");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
    }

    private bool IsAuthenticated()
    {
        return Preferences.Default.ContainsKey("hasAuth");
    }
}