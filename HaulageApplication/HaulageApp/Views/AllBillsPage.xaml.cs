using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class AllBillsPage : ContentPage
{
    public AllBillsPage(AllBillsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        billCollection.SelectedItem = null;
    }
}