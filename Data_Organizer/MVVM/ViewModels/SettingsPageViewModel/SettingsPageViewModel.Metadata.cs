using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SettingsPageViewModel
{
    public partial class SettingsPageViewModel
    {
        [RelayCommand]
        public async Task ShowTipAboutMetadata()
        {
            await _notificationService.ShowToastAsync("Геолокація, дата, час та" +
                                                      " пристрій реєстрації, авторизацій, виходів з акаунту" +
                                                      " та змінень паролю. Мета: " +
                                                      " відслідковування активності акаунту та " +
                                                      "потенційних підозрюваних дій.",
                                                      17);
        }
    }

}
