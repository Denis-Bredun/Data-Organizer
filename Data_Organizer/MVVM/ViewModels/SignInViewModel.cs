using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private bool _isLoading;

        public SignInViewModel(
            INotificationService notificationService,
            IGoogleAuthenticationService googleAuthenticationService)
        {
            _notificationService = notificationService;
            _googleAuthenticationService = googleAuthenticationService;
        }
    }
}