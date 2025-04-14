using Data_Organizer.MVVM.ViewModels.SettingsPageViewModel;

namespace Data_Organizer.MVVM.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsPageViewModel _settingsPageViewModel;

    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_settingsPageViewModel == null)
            _settingsPageViewModel = (SettingsPageViewModel)BindingContext;

        _settingsPageViewModel.IsLoading = false;
        _settingsPageViewModel.UpdateDataOnPage();

        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _settingsPageViewModel.UpdateDataOnPage();
        _settingsPageViewModel.Dispose();
    }
}