using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;
        private readonly IFirestoreDbService _firestoreDbService;
        private readonly IDeviceServiceDecorator _deviceServiceDecorator;

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
            IDeviceServiceDecorator deviceServiceDecorator)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            _firestoreDbService = firestoreDbService;
            _deviceServiceDecorator = deviceServiceDecorator;
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