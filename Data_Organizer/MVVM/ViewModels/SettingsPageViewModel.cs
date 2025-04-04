using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject, IDisposable
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private EventHandler _authStateChangedHandler;

        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private bool _isUserAuthorized;
        [ObservableProperty]
        private bool _isLoading;

        public SettingsPageViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
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
