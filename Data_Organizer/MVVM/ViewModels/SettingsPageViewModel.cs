using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject, IDisposable
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;

        [ObservableProperty] private string _username;
        [ObservableProperty] private bool _isUserAuthorized;
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private bool _isMetadataStored;

        private EventHandler _authStateChangedHandler;

        public SettingsPageViewModel(
            IFirebaseAuthService firebaseAuthService,
            INotificationService notificationService)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            _authStateChangedHandler = OnAuthStateChanged;
            _firebaseAuthService.AuthStateChanged += _authStateChangedHandler;

            UpdateDataOnPage();
        }

        [RelayCommand]
        public async Task NavigateToSignInPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        public async Task SignOut()
        {
            IsLoading = true;

            bool succeeded = await _firebaseAuthService.SignOut();

            if (succeeded)
                await Shell.Current.GoToAsync("//SignInPage");
            else
                IsLoading = false;
        }

        [RelayCommand]
        public async Task NavigateToResetPasswordPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//ResetPasswordPage");
        }

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

        [RelayCommand]
        public async Task DeleteAccount()
        {
            IsLoading = true;

            bool succeeded = await _firebaseAuthService.DeleteAccount();

            if (succeeded)
                await Shell.Current.GoToAsync("//SignInPage");
            else
                IsLoading = false;
        }

        private void OnAuthStateChanged(object sender, EventArgs e)
        {
            UpdateDataOnPage();
        }

        public void UpdateDataOnPage()
        {
            UpdateUsername();
            UpdateIsUserAuthorized();
        }

        private void UpdateUsername()
        {
            Username = _firebaseAuthService.GetUsername() ?? "Гість";
        }

        private void UpdateIsUserAuthorized()
        {
            IsUserAuthorized = _firebaseAuthService.IsUserAuthorized();
        }

        public void Dispose()
        {
            if (_authStateChangedHandler != null)
            {
                _firebaseAuthService.AuthStateChanged -= _authStateChangedHandler;
                _authStateChangedHandler = null;
            }
        }
    }

}
