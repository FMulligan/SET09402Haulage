using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;

namespace HaulageApp.ViewModels;

public partial class PermissionsViewModel : ObservableObject
{
    [ObservableProperty] private bool _manageCustomersIsVisible;
    [ObservableProperty] private bool _billsIsVisible;
    [ObservableProperty] private bool _tripsIsVisible;

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
                ManageCustomersIsVisible = false;
                BillsIsVisible = true;
                TripsIsVisible = false;
                break; 
            case 2:
                // Driver
                ManageCustomersIsVisible = false;
                BillsIsVisible = false;
                TripsIsVisible = true;
                break;
            case 3:
                // Admin
                ManageCustomersIsVisible = true;
                BillsIsVisible = false;
                TripsIsVisible = true;
                break;
        }
    }
}