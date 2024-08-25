using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class ManageCustomersPage : ContentPage
{
    private readonly ManageCustomersViewModel _viewModel;
    
    public ManageCustomersPage(ManageCustomersViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ClearCommand.Execute(null);
    }
}