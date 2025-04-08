using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class WelcomePage : ContentPage
{
    private WelcomeViewModel _welcomeViewModel;

    public WelcomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_welcomeViewModel == null)
            _welcomeViewModel = (WelcomeViewModel)BindingContext;

        _welcomeViewModel.IsLoading = false;

        base.OnAppearing();
    }
}