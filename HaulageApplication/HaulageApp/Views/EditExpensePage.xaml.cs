using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class EditExpensePage : ContentPage
{
    public EditExpensePage(EditExpenseViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}