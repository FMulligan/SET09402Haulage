using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Common;
using HaulageApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly HaulageDbContext _context;
    private readonly ISecureStorageWrapper _storageWrapper;

    public SettingsViewModel(HaulageDbContext dbContext, ISecureStorageWrapper storageWrapper)
    {
        _context = dbContext;
        _storageWrapper = storageWrapper;
        GetEmail();
    }

    private string? _email;
    public string Email
    {
        get => _email ?? "";
        set
        {
            _email = value;
            OnPropertyChanged(Email);
        }
    }

    private async Task GetEmail()
    {
        Email = await _storageWrapper.GetAsync("hasAuth") ?? "Error retrieving email.";
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        var newPassword = await Shell.Current.DisplayPromptAsync("Update Password", "Enter new password");
        // Result of "cancel" is null.
        if (newPassword == null)
        {
            return;
        }

        while (!PasswordIsValid(newPassword))
        {
            newPassword = await Shell.Current.DisplayPromptAsync("Error", "Choose a valid password");
            // Result of "cancel" is null.
            if (newPassword == null)
            {
                return;
            }
        }
        
        if (UpdateIsSuccessful(Email, newPassword))
        {
            await Shell.Current.DisplayAlert("", "Update successful", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert("","Something went wrong, try again.", "OK");
        }
    }

    private bool UpdateIsSuccessful(string email, string newPassword)
    {
        try
        {
           _context.user
                .AsQueryable()
                .Where(user => user.Email == email)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(user => user.Password, user => newPassword));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }

    [RelayCommand]
    private async Task Logout()
    {
        if (await Shell.Current.DisplayAlert("", "Log out of your account?", "Log out", "Cancel"))
        {
            SecureStorage.Remove("hasAuth");
            await Shell.Current.GoToAsync("///login");
        }
    }

    public bool PasswordIsValid(string? password)
    {
        // a pattern that may protect against sql injection, 
        // but this should be done on the server side
        const string pattern = @"[\s]*((delete)|(exec)|(drop\s*table)|(insert)|(shutdown)|(update)|(\bor\b))";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        if (regex.Match(password).Success)
        {
            return false;
        }

        // You could look/check for more.
        // e.g. at least x characters, include upper lower case, numbers, including special characters.
        return password.Trim().Length >= 6;
    }
}