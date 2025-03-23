using Data_Organizer.MVVM.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace Data_Organizer.MVVM.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue)
            await MyScrollView.ScrollToAsync(MyEditor, ScrollToPosition.End, true);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            SettingsScrollView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
            .SetIsLegacyColorModeEnabled(true);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}