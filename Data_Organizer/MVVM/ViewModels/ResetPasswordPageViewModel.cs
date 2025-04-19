using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class ResetPasswordPageViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;
        private readonly IFirestoreDbService _firestoreDbService;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _oldPassword;
        [ObservableProperty] private bool _isLoading;

        public SettingsPageViewModel.SettingsPageViewModel SettingsPageViewModel { get; set; }

        public ResetPasswordPageViewModel(
            IFirebaseAuthService firebaseAuthService,
            INotificationService notificationService,
            IFirestoreDbService firestoreDbService,
            IServiceProvider serviceProvider)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            _firestoreDbService = firestoreDbService;
            SettingsPageViewModel = serviceProvider.GetRequiredService<SettingsPageViewModel.SettingsPageViewModel>();
        }

        [RelayCommand]
        public async Task ChangePassword()
        {
            if (!await ValidateDataForEmptiness())
                return;

            Email = Email?.Trim();
            OldPassword = OldPassword?.Trim();

            if (!await ValidateEmailForCorrectnessIfAuthorized())
                return;

            if (!await UserReauthentication())
                return;

            if (!await ConfirmEmailIfNotAuthorized())
                return;

            IsLoading = true;

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

        private async Task<bool> ConfirmEmailIfNotAuthorized()
        {
            if (!SettingsPageViewModel.IsUserAuthorized)
            {
                bool emailConfirmed = await _notificationService.ShowConfirmationDialogAsync($"Ви точно правильно написали пошту?\n\n{Email}");

                return emailConfirmed;
            }

            return true;
        }

        private async Task<bool> ResetPassword()
        {
            return await _firebaseAuthService.ResetPassword(Email);
        }

        private async Task HandlePostResetActions()
        {
            if (SettingsPageViewModel.IsMetadataStored)
                await _firestoreDbService.SaveChangePasswordInstance(OldPassword);

            Email = "";
            OldPassword = "";

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

        private async Task<bool> ValidateDataForEmptiness()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пошту!");
                return false;
            }

            if (SettingsPageViewModel.IsUserAuthorized && string.IsNullOrWhiteSpace(OldPassword))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести старий пароль!");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateEmailForCorrectnessIfAuthorized()
        {
            if (SettingsPageViewModel.IsUserAuthorized && Email != _firebaseAuthService.GetEmail())
            {
                await _notificationService.ShowToastAsync("Уведена пошта не відповідає вашій реальній пошті!");
                return false;
            }

            return true;
        }

        private async Task<bool> UserReauthentication()
        {
            if (SettingsPageViewModel.IsUserAuthorized)
            {
                IsLoading = true;
                bool doesUserExist = await _firebaseAuthService.SignInAsync(Email, OldPassword);
                IsLoading = false;

                return doesUserExist;
            }

            return true;
        }
    }
}
