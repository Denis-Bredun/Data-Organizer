using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly INotificationService _notificationService;

        [ObservableProperty] private string _username;
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _confirmPassword;
        [ObservableProperty] private bool _isMetadataStored;
        [ObservableProperty] private bool _isLoading;

        public SignUpViewModel(
            IGoogleAuthenticationService googleAuthenticationService,
            INotificationService notificationService)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _notificationService = notificationService;
        }
    }
}