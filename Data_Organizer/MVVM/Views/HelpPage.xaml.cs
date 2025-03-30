using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.Views.Controls;

namespace Data_Organizer.MVVM.Views;

public partial class HelpPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public HelpPage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var helpPageViewModel = (HelpPageViewModel)BindingContext;
        helpPageViewModel.IsLoading = false;

        helpPageViewModel.ShowHomeHelpRequested += (s, e) => LoadHomeHelpSection();
        helpPageViewModel.ShowSavedNotesHelpRequested += (s, e) => LoadSavedNotesHelpSection();
        helpPageViewModel.ShowSettingsHelpRequested += (s, e) => LoadSettingsHelpSection();

        base.OnAppearing();

        LoadHomeHelpSection();
    }

    private void SetActiveTab(string tabName)
    {
        VisualStateManager.GoToState(HomeHelpButton, "Normal");
        VisualStateManager.GoToState(SavedNotesHelpButton, "Normal");
        VisualStateManager.GoToState(SettingsHelpButton, "Normal");

        switch (tabName)
        {
            case "Home":
                VisualStateManager.GoToState(HomeHelpButton, "Selected");
                break;
            case "SavedNotes":
                VisualStateManager.GoToState(SavedNotesHelpButton, "Selected");
                break;
            case "Settings":
                VisualStateManager.GoToState(SettingsHelpButton, "Selected");
                break;
        }
    }

    private void LoadHomeHelpSection()
    {
        SetActiveTab("Home");
        var homeHelpSection = _serviceProvider?.GetService<HomePageHelpSection>();
        ActiveHelpSection.Content = homeHelpSection;
    }

    private void LoadSavedNotesHelpSection()
    {
        SetActiveTab("SavedNotes");
        var savedNotesHelpSection = _serviceProvider?.GetService<SavedNotesHelpSection>();
        ActiveHelpSection.Content = savedNotesHelpSection;
    }

    private void LoadSettingsHelpSection()
    {
        SetActiveTab("Settings");
        var settingsHelpSection = _serviceProvider?.GetService<SettingsHelpSection>();
        ActiveHelpSection.Content = settingsHelpSection;
    }
}