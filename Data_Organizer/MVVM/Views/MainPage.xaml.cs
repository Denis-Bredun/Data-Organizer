using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
namespace Data_Organizer.MVVM.Views;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            SettingsScrollView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
            .SetIsLegacyColorModeEnabled(true);
        }

        if (VersionTracking.IsFirstLaunchEver)
            ((MainPageViewModel)BindingContext).IsHelpOpen = true;
    }

    private async void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue)
            await MyScrollView.ScrollToAsync(MyEditor, ScrollToPosition.End, true);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}