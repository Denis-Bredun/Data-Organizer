using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;
        private readonly IFirestoreDbService _firestoreDbService;
        private readonly IDeviceServiceDecorator _deviceServiceDecorator;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty] private string _username;
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _confirmPassword;
        [ObservableProperty] private bool _isMetadataStored;
        [ObservableProperty] private bool _isLoading;

        public SignUpViewModel(
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
            _serviceProvider = serviceProvider;
        }

        private void CleanFields()
        {
            Email = "";
            Password = "";
            ConfirmPassword = "";
            Username = "";
            IsMetadataStored = false;
        }
    }
}