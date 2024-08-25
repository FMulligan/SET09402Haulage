using HaulageApp.ViewModels;

namespace HaulageApp.Views
{
    public partial class TripPage : ContentPage
    {
        private readonly TripViewModel _viewModel;

        public TripPage(TripViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.LoadDataAsync();
            }
        }
    }
}