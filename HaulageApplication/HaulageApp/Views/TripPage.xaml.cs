using HaulageApp.ViewModels;

namespace HaulageApp.Views
{
    public partial class TripPage : ContentPage
    {
        public TripPage(TripViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}