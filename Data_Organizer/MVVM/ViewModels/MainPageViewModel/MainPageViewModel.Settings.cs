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
        public void ChangeSettingAreHeadersAdded()
        {
            AreHeadersAdded = !AreHeadersAdded;
        }

        [RelayCommand]
        public async Task ShowHelpInformation()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//HelpPage");

            IsLoading = false;
        }

        private void AddHeaders(ref string content, string fileOrFeatureName)
        {
            string dateOfConversion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            content = $"Назва функції/файлу: {fileOrFeatureName}\n" +
                      $"Дата операції/імпорту: {dateOfConversion}\n" +
                      $"Текст:\n{content}";
        }
    }
}