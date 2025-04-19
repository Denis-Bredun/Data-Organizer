using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SignInViewModel
{
    public partial class SignInViewModel
    {
        [RelayCommand]
        public async Task SignIn()
        {
            if (!await ValidateDataForEmptiness())
                return;

            IsLoading = true;

            Email = Email?.Trim();
            Password = Password?.Trim();

            bool succeeded = await _firebaseAuthService.SignInAsync(Email, Password);

            if (succeeded)
            {
                await SetMetadataFlagIfAuthorized();

                if (_settingsPageViewModel.IsMetadataStored)
                    await _firestoreDbService.CreateAccountLoginInstance();

                CleanFields();

                await NavigateToMainPage();
            }
            else
                IsLoading = false;
        }

        private async Task<bool> ValidateDataForEmptiness()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пошту!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пароль!");
                return false;
            }

            return true;
        }
    }
}