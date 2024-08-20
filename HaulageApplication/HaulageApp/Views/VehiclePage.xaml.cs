using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class VehiclePage : ContentPage
{
    public VehiclePage(VehicleViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}