using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
namespace Data_Organizer.MVVM.Views;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

public partial class MainPage : ContentPage
{
    private bool _wasHelpPageClosed;

    public MainPage()
    {
        VersionTracking.Track();   

        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var mainPageViewModel = (MainPageViewModel)BindingContext;
        mainPageViewModel.IsLoading = false;
        mainPageViewModel.HasVisitedMainPage = true;

        ConfigurePlatformSpecifics();
        
        await CheckAndShowHelpIfNeeded(mainPageViewModel);
    }

    private void ConfigurePlatformSpecifics()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            SettingsScrollView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .SetIsLegacyColorModeEnabled(true);
        }
    }

    private async Task CheckAndShowHelpIfNeeded(MainPageViewModel viewModel)
    {
        if (VersionTracking.IsFirstLaunchEver && !_wasHelpPageClosed)
        {
            await viewModel.ShowHelpInformation();
            _wasHelpPageClosed = true;
        }
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