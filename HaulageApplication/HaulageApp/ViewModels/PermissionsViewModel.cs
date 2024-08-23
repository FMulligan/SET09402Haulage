using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Data;
using HaulageApp.Models;

namespace HaulageApp.ViewModels;

public partial class PermissionsViewModel(HaulageDbContext dbContext) : ObservableObject
{
    [ObservableProperty] private bool _manageCustomersIsVisible;
    // example (using notes)
    [ObservableProperty] private bool _notesIsVisible;

    public void UpdateTabsForCurrentUser()
    {
        var username = Preferences.Get("hasAuth", "");
        var user = GetUser(username);
        if (user != null)
        {
            SetVisibilityByRole(user.Role);
        }
    }
    
    public void UpdateTabsForCurrentUser(int roleId)
    {
        SetVisibilityByRole(roleId);
    }

    private User? GetUser(string username)
    {
        return dbContext.user
            .AsQueryable()
            .FirstOrDefault(u => u.Email == username.ToLower());
    }
    
    private void SetVisibilityByRole(int roleId)
    {
        // It would be better to check the role name rather than ID.
        // This just requires an additional call to the db to check 
        // the role name against id. For simplicity just adding this for now.
        switch (roleId)
        {
            default:
                // Customer (and anyone else)
                NotesIsVisible = false;
                ManageCustomersIsVisible = false;
                break; 
            case 2:
                // Driver
                NotesIsVisible = true;
                ManageCustomersIsVisible = false;
                break;
            case 3:
                // Admin
                NotesIsVisible = false;
                ManageCustomersIsVisible = true;
                break;
        }
    }
}