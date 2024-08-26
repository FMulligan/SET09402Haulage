using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class EditCustomerPage : ContentPage
{
    private readonly EditCustomerViewModel _viewModel;
    
    public EditCustomerPage(EditCustomerViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    private void Picker_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        _viewModel.SelectStatusCommand.Execute(sender);
    }
}