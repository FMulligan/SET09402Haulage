using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class AllItemsPage : ContentPage
{
    private AllItemsViewModel _viewModel;
    public AllItemsPage(AllItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAllItemsForCustomer();
    }
}