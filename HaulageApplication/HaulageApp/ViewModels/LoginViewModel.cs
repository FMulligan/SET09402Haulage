using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly HaulageDbContext _context;
    private readonly PermissionsViewModel _permissionsViewModel;

    public LoginViewModel(HaulageDbContext dbContext, PermissionsViewModel permissionsViewModel)
    {
        _context = dbContext;
        _permissionsViewModel = permissionsViewModel;
    }

    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    
    public User? User { get; set; }

    [RelayCommand]
    private async Task Login()
    {
        var isCredentialCorrect = false;
        var connected = true;

        try
        {
            User = GetUser(Email);
            if (User != null)
            {
                isCredentialCorrect = IsCredentialCorrect(User, Password);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            connected = false;
            await Shell.Current.DisplayAlert("Login failed", "Ensure you are connected to the internet", "Try again");
        }

        switch (isCredentialCorrect)
        {
            case true:
                Preferences.Default.Set("hasAuth", Email);
                // we could also set the role in user defaults here if needed 
                Preferences.Set("currentUserId", User.Id);
                Preferences.Set("currentUserRole", User.Role);
                _permissionsViewModel.UpdateTabsForCurrentUser(User!.Role);
                // as mentioned on LoadingPage, this should either be a page that all roles have access to,
                // e.g. settings. Another option is to go to a different page based on which role is logged in.
                // (e.g. switch case based on role)
                await Shell.Current.GoToAsync("///settings");
                break;
            case false when connected:
                await Shell.Current.DisplayAlert("Login failed", "Username or password is incorrect", "Try again");
                break;
        }
    }

    private User? GetUser(string username)
    {
        return _context.user
            .AsQueryable()
            .FirstOrDefault(u => u.Email == username.ToLower());
    }

    public bool IsCredentialCorrect(User user, string password)
    {
        if (user.Password == password)
        {
            return true;
        }
        return false;
    }
}