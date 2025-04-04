using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly INotificationService _notificationService;

        [ObservableProperty] private string _username;
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _confirmPassword;
        [ObservableProperty] private bool _isMetadataStored;
        [ObservableProperty] private bool _isLoading;

        public SignUpViewModel(
            IFirebaseAuthService firebaseAuthService,
            INotificationService notificationService)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
        }

        [RelayCommand]
        public async Task ShowTipAboutMetadata()
        {
            await _notificationService.ShowToastAsync("Метадані включають в себе: геолокація, дата, час та" +
                                                      " пристрій реєстрації, авторизацій, виходів з акаунту" +
                                                      " та змінею паролю. Збір буде здійснюватись із метою " +
                                                      " можливості відслідковування активності акаунту та " +
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