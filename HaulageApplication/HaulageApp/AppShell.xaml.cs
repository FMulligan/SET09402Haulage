using HaulageApp.ViewModels;
using HaulageApp.Views;

namespace HaulageApp;

public partial class AppShell : Shell
{
    public AppShell(PermissionsViewModel viewmodel)
    {
        BindingContext = viewmodel;
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("vehicles", typeof(AllVehiclesPage));
        Routing.RegisterRoute("expenses", typeof(ExpensesPage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
        Routing.RegisterRoute("events", typeof(AllEventsPage));
        Routing.RegisterRoute("items", typeof(AllItemsPage));
        Routing.RegisterRoute(nameof(VehiclePage), typeof(VehiclePage));
        Routing.RegisterRoute(nameof(EditExpensePage), typeof(EditExpensePage));
        Routing.RegisterRoute(nameof(AllBillsPage), typeof(AllBillsPage));
        Routing.RegisterRoute(nameof(EditCustomerPage), typeof(EditCustomerPage));
        Routing.RegisterRoute(nameof(EventPage), typeof(EventPage));
        Routing.RegisterRoute(nameof(TripPage), typeof(TripPage));
        Routing.RegisterRoute(nameof(ItemPage), typeof(ItemPage));
    }
}