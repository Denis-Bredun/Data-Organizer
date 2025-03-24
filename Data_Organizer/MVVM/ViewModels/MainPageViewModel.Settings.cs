using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels
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
            // Implementation for showing help information
        }
    }
} 