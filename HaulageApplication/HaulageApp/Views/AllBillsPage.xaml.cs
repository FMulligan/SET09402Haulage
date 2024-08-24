using HaulageApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace HaulageApp.Views;

public partial class AllBillsPage : ContentPage
{
    private readonly AllBillsViewModel _viewModel;
    public AllBillsPage(AllBillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel; 
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBillsAsync();
    }
}