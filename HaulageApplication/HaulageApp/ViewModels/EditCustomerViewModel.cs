using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels;

public partial class EditCustomerViewModel : ObservableObject, IQueryAttributable
{
    private User? _customer;
    private readonly HaulageDbContext _context;
    private readonly INavigationService _navigationService;
    private string _email;
    private int _statusIndex;
    private string _status;
    
    public EditCustomerViewModel(HaulageDbContext dbContext, INavigationService navigationService)
    {
        _context = dbContext;
        _navigationService = navigationService;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("customer"))
        {
            _customer = query["customer"] as User;
            Email = _customer.Email;
            StatusIndex = GetIndexForStatus(_customer.Status);
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }
    
    public int StatusIndex
    {
        get => _statusIndex;
        set
        {
            _statusIndex = value;
            OnPropertyChanged();
        }
    }

    private int GetIndexForStatus(string customerStatus)
    {
        if (customerStatus.ToLower().Equals("active"))
        {
            return 0;
        }
        return 1;
    }
    
    [RelayCommand]
    private void SelectStatus(object sender)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            StatusIndex = selectedIndex;
            _status = picker.Items[selectedIndex].ToLower();
        }
    }
    
    [RelayCommand]
    private async Task Save()
    {
        Update(_customer, _status);
        await _context.Entry(_customer).ReloadAsync();
        await _navigationService.GoToAsync("..", new Dictionary<string, object>{{ "reload", "" }});
    }
    
    private void Update(User customer, string status)
    {
        try
        {
            _context.user.AsQueryable()
                .Where(u => u.Id == customer.Id)
                .ExecuteUpdate(s => 
                    s.SetProperty(e => e.Status, e => status)
                );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}