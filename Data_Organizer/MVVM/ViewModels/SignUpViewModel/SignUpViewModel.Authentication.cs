using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel
    {
        [RelayCommand]
        public async Task SignUp()
        {
            if (!await ValidateData())
                return;

            IsLoading = true;

            Data_Organizer.MVVM.Models.Location location = await GetLocationForMetadata();
            if (location == null)
                return;

            bool succeeded = await RegisterUser();
            if (succeeded)
            {
                await CreateUserInDatabase(location);
                await NavigateToMainPage();
            }
            else
            {
                IsLoading = false;
            }
        }

        private async Task<Data_Organizer.MVVM.Models.Location> GetLocationForMetadata()
        {
            if (!IsMetadataStored)
                return null;

            var location = await _deviceServiceDecorator.GetCurrentLocationAsync();
            if (location == null)
            {
                IsLoading = false;
                await _notificationService.ShowToastAsync("Для збору метаданих потрібен доступ до вашої геолокації!");
            }
            return location;
        }

        private async Task<bool> RegisterUser()
        {
            return await _firebaseAuthService.SignUpAsync(Email, Password, Username);
        }

        private async Task CreateUserInDatabase(Data_Organizer.MVVM.Models.Location location)
        {
            await _firestoreDbService.CreateUserAsync(IsMetadataStored, location);

            var settingsPageViewModel = _serviceProvider.GetRequiredService<SettingsPageViewModel.SettingsPageViewModel>();
            settingsPageViewModel.ChangeMetadataFlagWithoutAsking(IsMetadataStored);

            CleanFields();
        }
    }
}