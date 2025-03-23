using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private bool _isLoading;

        public SignInViewModel(
            INotificationService notificationService,
            IGoogleAuthenticationService googleAuthenticationService)
        {
            _notificationService = notificationService;
            _googleAuthenticationService = googleAuthenticationService;
        }

        [RelayCommand]
        public async Task NavigateToSignUpPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//SignUpPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            // добавить сохранение куда-то того, что метаданные не собираются

            IsLoading = true;

            await Shell.Current.GoToAsync("//TabBar");
        }

        [RelayCommand]
        public async Task SignIn()
        {
            if (!await ValidateDataForEmptiness())
                return;

            IsLoading = true;

            Email = Email?.Trim();
            Password = Password?.Trim();

            bool succeeded = await _googleAuthenticationService.SignInAsync(Email, Password);

            if (succeeded)
                await NavigateToMainPage();
            else
                IsLoading = false;

            // подгрузка о том, чи собираем метаданные
        }

        private async Task<bool> ValidateDataForEmptiness()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пошту!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await _notificationService.ShowToastAsync("Потрібно ввести пароль!");
                return false;
            }

            return true;
        }
    }
}
