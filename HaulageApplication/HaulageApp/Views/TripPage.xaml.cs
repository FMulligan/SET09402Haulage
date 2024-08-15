using HaulageApp.ViewModels;
using Microsoft.Maui.Controls;

namespace HaulageApp.Views
{
    public partial class TripPage : ContentPage
    {
        public TripPage(TripViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}