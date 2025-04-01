using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SignInViewModel
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private bool _isLoading;

        public SignInViewModel(
            INotificationService notificationService,
            IFirebaseAuthService firebaseAuthService)
        {
            _notificationService = notificationService;
            _firebaseAuthService = firebaseAuthService;
        }
    }
}