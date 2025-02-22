using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;
using System.Text.RegularExpressions;

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
            await Shell.Current.GoToAsync("//SignUpPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            // добавить сохранение куда-то того, что метаданные не собираются

            await Shell.Current.GoToAsync("//TabBar");
        }

        [RelayCommand]
        public async Task SignIn()
        {
            if (!await ValidateData())
                return;

            IsLoading = true;

            bool succeeded = await _googleAuthenticationService.SignInAsync(Email, Password);

            IsLoading = false;

            if (succeeded)
                await NavigateToMainPage();

            // подгрузка о том, чи собираем метаданные
        }

        private async Task<bool> ValidateData()
        {
            return await ValidateDataForEmptiness() &&
                   await ValidateEmail() &&
                   await ValidatePassword();
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

        private async Task<bool> ValidateEmail()
        {
            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                await _notificationService.ShowToastAsync("Невірний формат електронної пошти!");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidatePassword()
        {
            if (!Regex.IsMatch(Password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[\W_]).{6,}$"))
            {
                await _notificationService.ShowToastAsync("Пароль повинен містити щонайменше 6 символів, включаючи хоча б одну літеру, одну цифру та один спеціальний символ.");
                return false;
            }

            return true;
        }
    }
}
