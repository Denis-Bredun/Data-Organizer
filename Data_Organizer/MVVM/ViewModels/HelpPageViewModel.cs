using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class HelpPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private bool _hasBeenClosedOnce;

        public event EventHandler ShowHomeHelpRequested;
        public event EventHandler ShowSavedNotesHelpRequested;
        public event EventHandler ShowSettingsHelpRequested;

        public HelpPageViewModel()
        {
        }

        [RelayCommand]
        public async Task CloseHelpAndNavigateToMainPage()
        {
            IsLoading = true;
            HasBeenClosedOnce = true;
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        public void ShowHomeHelp()
        {
            ShowHomeHelpRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        public void ShowSavedNotesHelp()
        {
            ShowSavedNotesHelpRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        public void ShowSettingsHelp()
        {
            ShowSettingsHelpRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}