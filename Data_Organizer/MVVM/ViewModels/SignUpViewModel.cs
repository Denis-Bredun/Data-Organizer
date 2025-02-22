using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;
using System.Text.RegularExpressions;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
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

        public SignUpViewModel(
            IGoogleAuthenticationService googleAuthenticationService,
            INotificationService notificationService)
        {
            _googleAuthenticationService = googleAuthenticationService;
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
            // добавить сохранение куда-то того, что метаданные не собираются

            await Shell.Current.GoToAsync("//TabBar");
        }

        [RelayCommand]
        public async Task SignUp()
        {
            if (!await ValidateData())
                return;

            bool succeeded = await _googleAuthenticationService.SignUpAsync(Email, Password, Username);

            if (succeeded)
                await NavigateToMainPage();

            // добавить сохранение куда-то того, что метаданные собираются
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

        private async Task<bool> ValidateData()
        {
            return await ValidateDataForEmptiness() &&
                   await ValidateUsername() &&
                   await ValidateEmail() &&
                   await ValidatePassword() &&
                   await ValidateEqualityOfTheEnteredPasswords();
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

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await _notificationService.ShowToastAsync("Потрібно підтвердити пароль!");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateEqualityOfTheEnteredPasswords()
        {
            if (Password != ConfirmPassword)
            {
                await _notificationService.ShowToastAsync("Уведені паролі не збігаються!");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                return true;
            }

            if (!Regex.IsMatch(Username, "^[A-Za-z][A-Za-z0-9_]{2,}$"))
            {
                await _notificationService.ShowToastAsync("Ім'я користувача повинно починатися з літери, містити щонайменше 3 символи, з яких мінімум 2 — літери. Допустимі символи: англійські літери, цифри та нижнє підкреслення. Пробіли заборонені.");
                return false;
            }

            if (Username.Contains(" "))
            {
                await _notificationService.ShowToastAsync("Ім'я користувача не повинно містити пробілів.");
                return false;
            }

            int letterCount = Username.Count(char.IsLetter);
            if (letterCount < 2)
            {
                await _notificationService.ShowToastAsync("Ім'я користувача повинно містити щонайменше 2 літери.");
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
