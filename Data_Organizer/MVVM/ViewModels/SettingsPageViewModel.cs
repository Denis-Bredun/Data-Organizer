using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject, IDisposable
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private EventHandler _authStateChangedHandler;

        [ObservableProperty]
        private string _username;

        public SettingsPageViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
            _authStateChangedHandler = OnAuthStateChanged;
            _firebaseAuthService.AuthStateChanged += _authStateChangedHandler;
            UpdateUsername();
        }

        private void OnAuthStateChanged(object sender, EventArgs e)
        {
            UpdateUsername();
        }

        private void UpdateUsername()
        {
            Username = _firebaseAuthService.GetUsername() ?? "Гість";
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
