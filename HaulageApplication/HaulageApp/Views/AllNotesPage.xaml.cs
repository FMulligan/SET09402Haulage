using HaulageApp.ViewModels;

namespace HaulageApp.Views;

public partial class AllNotesPage : ContentPage
{
    public AllNotesPage(AllNotesViewModel viewModel)
    {
        BindingContext = viewModel;   
        InitializeComponent();
    }
    
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}