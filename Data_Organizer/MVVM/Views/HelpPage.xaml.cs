using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var helpPageViewModel = (HelpPageViewModel)BindingContext;

        helpPageViewModel.IsLoading = false;

        base.OnAppearing();
    }
} 