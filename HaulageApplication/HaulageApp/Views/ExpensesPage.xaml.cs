using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class ExpensesPage : ContentPage
{
    public ExpensesPage(ExpensesViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}