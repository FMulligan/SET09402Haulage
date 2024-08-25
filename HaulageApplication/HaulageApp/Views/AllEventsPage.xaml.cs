using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class AllEventsPage : ContentPage
{
    public AllEventsPage(AllEventsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
    
}