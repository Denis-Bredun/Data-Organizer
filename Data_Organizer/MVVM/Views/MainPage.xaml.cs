using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
namespace Data_Organizer.MVVM.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (VersionTracking.IsFirstLaunchEver)
            ((MainPageViewModel)BindingContext).IsHelpOpen = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}