using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Models;
using HaulageApp.Data;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly HaulageDbContext _context;

    public LoginViewModel(HaulageDbContext dbContext)
    {
        _context = dbContext;
    }

    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    [RelayCommand]
    private async Task Login()
    {
        var isCredentialCorrect = false;
        var connected = true;

        try
        {
            isCredentialCorrect = IsCredentialCorrect(Email, Password);
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
                await SecureStorage.SetAsync("hasAuth", Email);
                await Shell.Current.GoToAsync("///home");
                break;
            case false when connected:
                await Shell.Current.DisplayAlert("Login failed", "Username or password is incorrect", "Try again");
                break;
        }
    }

    public bool IsCredentialCorrect(string username, string password)
    {
        User? user = _context.user
            .AsQueryable()
            .FirstOrDefault(user => user.Email == username.ToLower() && user.Password == password);
        return user != null;
    }
}