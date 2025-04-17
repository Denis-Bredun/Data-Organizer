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

        partial void OnIsMetadataStoredChanged(bool oldValue, bool newValue)
        {
            if (_isReverting) return;
            HandleMetadataChangeAsync(oldValue, newValue);
        }

        private async void HandleMetadataChangeAsync(bool oldValue, bool newValue)
        {
            var message = newValue
                ? "Дозволити збір геолокації та даних пристрою для аналізу підозрілої активності?"
                : "Зупинити збір метаданих? Це обмежить можливість аналізу підозрілої активності.";

            var confirmed = await _notificationService.ShowConfirmationDialogAsync(message);

            if (confirmed)
            {
                IsLoading = true;
                await _firestoreDbService.SetMetadataStoredAsync(newValue);
                IsLoading = false;
            }
            else
            {
                _isReverting = true;
                IsMetadataStored = oldValue;
                _isReverting = false;
            }
        }

        public void ChangeMetadataFlagWithoutAsking(bool isMetadataStored)
        {
            _isReverting = true;
            IsMetadataStored = isMetadataStored;
            _isReverting = false;
        }
    }

}
