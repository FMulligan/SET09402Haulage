using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class ManageCustomersPage : ContentPage
{
    public ManageCustomersPage(ManageCustomersViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}