using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel
    {
        [RelayCommand]
        public void OpenCloseSettings()
        {
            AreSettingsOpen = !AreSettingsOpen;
        }

        [RelayCommand]
        public void ChangeSettingIsTextAddedAtTheEnd()
        {
            IsTextAddedAtTheEnd = !IsTextAddedAtTheEnd;
        }

        [RelayCommand]
        public async Task ShowHelpInformation()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//HelpPage");

            IsLoading = false;
        }
    }
}