using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _confirmPassword;
        [ObservableProperty]
        private bool _isMetadataStored;

        public SignUpViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [RelayCommand]
        public async Task NavigateToSignInPage()
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            await Shell.Current.GoToAsync("//TabBar");
        }

        [RelayCommand]
        public async Task ShowTipAboutMetadata()
        {
            await _notificationService.ShowToastAsync("Метадані включають в себе: геолокація, дата, час та" +
                                                      " пристрій реєстрації, авторизацій, виходів з акаунту" +
                                                      " та змінею паролю. Збір буде здійснюватись із метою " +
                                                      " можливості відслідковування активності акаунту та" +
                                                      "потенційних підозрюваних дій.",
                                                      17);
        }
    }
}
