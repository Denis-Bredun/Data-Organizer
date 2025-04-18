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

            content = $"����� �������/�����: {fileOrFeatureName}\n" +
                      $"���� ��������/�������: {dateOfConversion}\n" +
                      $"�����:\n{content}";
        }
    }
}