using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class AllVehiclesPage : ContentPage
{
    public AllVehiclesPage(AllVehiclesViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        vehicleCollection.SelectedItem = null;
    }
}