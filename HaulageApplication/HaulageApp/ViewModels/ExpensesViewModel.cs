using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;

namespace HaulageApp.ViewModels;

public partial class ExpensesViewModel : ObservableObject, IQueryAttributable
{
    private readonly HaulageDbContext _context;
    private Trip? _trip;
    private Expense _expense;
    public ObservableCollection<Expense> Expenses { get; } = [];

    public ExpensesViewModel(HaulageDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("trip"))
        {
            _trip = query["trip"] as Trip;
        }
        Expenses.Clear();
        var expenses = GetAllExpensesForTrip(_trip.Id);
        foreach (var expense in expenses)
        {
            Expenses.Add(expense);
        }
    }
    
    private List<Expense>? GetAllExpensesForTrip(int tripId)
    {
        var expenses = _context.expense
                    .AsQueryable()
                    .Where(expense => expense.Trip == tripId)
                    .ToList();
        return expenses;
    }
    
    [RelayCommand]
    private async Task AddExpense()
    {
        await Shell.Current.GoToAsync(nameof(Views.EditExpensePage), new Dictionary<string, object>{{ "trip", _trip! }});
    }
    
    [RelayCommand]
    private async Task SelectExpense(object item)
    {
        await Shell.Current.GoToAsync(nameof(Views.EditExpensePage), new Dictionary<string, object>{{ "trip", _trip! }, { "expense", item }});
    }
}