using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Common;
using HaulageApp.Data;
using HaulageApp.Models;

namespace HaulageApp.ViewModels;

public partial class PermissionsViewModel : ObservableObject
{
    [ObservableProperty] private bool _manageCustomersIsVisible;
    // example (using notes)
    [ObservableProperty] private bool _notesIsVisible;
    [ObservableProperty] private bool _billsIsVisible;
    
    private readonly HaulageDbContext _dbContext;
    private readonly IPreferencesWrapper _preferencesWrapper;

    public PermissionsViewModel(HaulageDbContext dbContext, IPreferencesWrapper preferencesWrapper)
    {
        _dbContext = dbContext;
        _preferencesWrapper = preferencesWrapper;
        UpdateTabsForCurrentUser();
    }

    public void UpdateTabsForCurrentUser()
    {
        var username = _preferencesWrapper.Get<string>("hasAuth", "");
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
        return _dbContext.user
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
                BillsIsVisible = true;
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