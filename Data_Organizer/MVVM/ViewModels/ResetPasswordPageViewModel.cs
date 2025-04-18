using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class ResetPasswordPageViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _oldPassword;
        [ObservableProperty] private bool _isLoading;

        public ResetPasswordPageViewModel(
            IFirebaseAuthService firebaseAuthService,
            INotificationService notificationService)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
        }

        [RelayCommand]
        public async Task ChangePassword()
        {
            if (!await ValidateEmailForEmptiness())
                return;

            IsLoading = true;

            bool confirmed = await ConfirmEmail();
            if (!confirmed)
            {
                IsLoading = false;
                return;
            }

            Email = Email?.Trim();

            bool succeeded = await ResetPassword();
            if (succeeded)
            {
                await HandlePostResetActions();
            }
            else
            {
                IsLoading = false;
            }
        }

        private async Task<bool> ConfirmEmail()
        {
            return await _notificationService.ShowConfirmationDialogAsync($"Ви точно правильно написали пошту?\n\n{Email}");
        }

        private async Task<bool> ResetPassword()
        {
            return await _firebaseAuthService.ResetPassword(Email);
        }

        private async Task HandlePostResetActions()
        {
            Email = "";

            if (_firebaseAuthService.IsUserAuthorized())
            {
                bool signOutSucceeded = await _firebaseAuthService.SignOutWithoutNotification();

                if (signOutSucceeded)
                    await Shell.Current.GoToAsync("//SignInPage");
                else
                    await Shell.Current.GoToAsync("//SettingsPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//SignInPage");
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            IsLoading = true;

            if (_firebaseAuthService.IsUserAuthorized())
                await Shell.Current.GoToAsync("//SettingsPage");
            else
                await Shell.Current.GoToAsync("//SignInPage");
        }

        private async Task<bool> ValidateEmailForEmptiness()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пошту!");
                return false;
            }

            return true;
        }
    }
}
