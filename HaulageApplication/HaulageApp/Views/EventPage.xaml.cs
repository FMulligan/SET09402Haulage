using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class EventPage : ContentPage
{
    public EventPage(EventViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}