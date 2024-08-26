using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.Services;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels;

public partial class EditExpenseViewModel: ObservableObject, IQueryAttributable
{
    private Expense? _expense;
    private Trip? _trip;
    private readonly HaulageDbContext _context;
    private readonly INavigationService _navigationService;
    
    public EditExpenseViewModel(HaulageDbContext dbContext, INavigationService navigationService)
    {
        _context = dbContext;
        _navigationService = navigationService;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("trip"))
        {
            _trip = query["trip"] as Trip;
        }

        if (query.ContainsKey("expense"))
        {
            _expense = query["expense"] as Expense;
            Amount = _expense.Amount;
        }
    }

    private Decimal? _amount;
    public Decimal? Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    public async Task Save()
    {
        var amount  = Amount ?? 0;
        if (AmountIsValid(amount))
        {
            if (_expense == null)
            {
                _expense = new Expense { Trip = _trip.Id, Driver = _trip.Driver, Amount = amount, Status = "pending"};
                Add(_expense);
            }
            else
            {
                Update(_expense);
                await _context.Entry(_expense).ReloadAsync();
            }
            await _navigationService.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Invalid Input", "You must specify a number.", "OK");
        }
    }

    private bool AmountIsValid(Decimal amount)
    {
        return amount != 0;
    }

    private void Add(Expense expense)
    {
        try
        {
            _context.expense.Add(expense);
            _context.SaveChanges();
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private void Update(Expense expense)
    {
        try
        {
            _context.expense
                .AsQueryable()
                .Where(e => e.Id == expense.Id)
                .ExecuteUpdate(s => 
                    s.SetProperty(e => e.Amount, e => Amount)
                );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    [RelayCommand]
    public async Task Delete()
    {
        DeleteFromDb(_expense);
        await _navigationService.GoToAsync("..");
    }

    private void DeleteFromDb(Expense expense)
    {
        try
        {
            _context.expense.AsQueryable().Where(e => e.Id == expense.Id).ExecuteDelete();
        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}