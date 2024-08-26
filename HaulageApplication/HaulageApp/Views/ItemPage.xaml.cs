using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class ItemPage : ContentPage
{
    public ItemPage(ItemViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}