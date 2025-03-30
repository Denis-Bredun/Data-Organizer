using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
namespace Data_Organizer.MVVM.Views;

public partial class MainPage : ContentPage
{
    private static bool _isHelpShown = false;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (VersionTracking.IsFirstLaunchEver && !_isHelpShown)
        {
            ((MainPageViewModel)BindingContext).IsHelpOpen = true;
            _isHelpShown = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}