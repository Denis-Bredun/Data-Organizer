using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var welcomePageViewModel = (WelcomeViewModel)BindingContext;

        welcomePageViewModel.IsLoading = false;

        base.OnAppearing();
    }
}