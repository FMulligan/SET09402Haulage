using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;

namespace HaulageApp.ViewModels;

public partial class ManageCustomersViewModel : ObservableObject
{
    private readonly HaulageDbContext _context;
    private Role? _role;
    private List<User> _allCustomers;
    public ObservableCollection<User> Customers { get; } = [];
    
    public ManageCustomersViewModel(HaulageDbContext dbContext)
    {
        _context = dbContext;
        LoadAllCustomers();
    }
    
    private string _customerIdOrEmail;
    public string CustomerIdOrEmail
    {
        get => _customerIdOrEmail;
        set
        {
            _customerIdOrEmail = value;
            OnPropertyChanged();
        }
    }

    private void LoadAllCustomers()
    {
        _allCustomers = GetAllCustomers();
        foreach (var customer in _allCustomers)
        {
            Customers.Add(customer);
        }
    }

    private List<User> GetAllCustomers()
    {
        Role? customerRole = null;
        
        try
        {
           customerRole = _context.role
                .FirstOrDefault(role => role.Type == "customer");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // could feed the UI some error about connection issues
        }

        if (customerRole != null)
        {
            try
            {
                var customers = _context.user
                    .Where(user => user.Role == customerRole.Id)
                    .ToList();
                return customers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // as above
            }
        }
        
        return new List<User>();
    }

    [RelayCommand]
    private void Search()
    {
        var customer = IdOrEmailMatchesCustomer();
        if (customer != null)
        {
            Customers.Clear();
            Customers.Add(customer);
        }
        else
        {
            Customers.Clear();
        }
    }

    private User? IdOrEmailMatchesCustomer()
    {
        foreach (var customer in _allCustomers)
        {
            if (customer.Id.ToString() == CustomerIdOrEmail 
                || customer.Email == CustomerIdOrEmail.ToLower())
            {
                return customer;
            }
        }
        return null;
    }
    
    [RelayCommand]
    private void Clear()
    {
        Customers.Clear();
        foreach (var customer in _allCustomers)
        {
            Customers.Add(customer);
        }
    }
}