using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SettingsPageViewModel
{
    public partial class SettingsPageViewModel : ObservableObject, IDisposable
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;
        private readonly IFirestoreDbService _firestoreDbService;
        private readonly IDeviceServiceDecorator _deviceServiceDecorator;
        private readonly SavedNotesPageViewModel _savedNotesPageViewModel;

        private bool _isReverting;

        [ObservableProperty] private string _username;
        [ObservableProperty] private bool _isUserAuthorized;
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private bool _isMetadataStored;

        private EventHandler _authStateChangedHandler;

        public SettingsPageViewModel(
            IFirebaseAuthService firebaseAuthService,
            INotificationService notificationService,
            IFirestoreDbService firestoreDbService,
            IDeviceServiceDecorator deviceServiceDecorator,
            IServiceProvider serviceProvider)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            _firestoreDbService = firestoreDbService;
            _deviceServiceDecorator = deviceServiceDecorator;
            _savedNotesPageViewModel = serviceProvider.GetRequiredService<SavedNotesPageViewModel>();

            _authStateChangedHandler = OnAuthStateChanged;
            _firebaseAuthService.AuthStateChanged += _authStateChangedHandler;

            UpdateDataOnPage();
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
