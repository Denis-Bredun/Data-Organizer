using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignUpViewModel
    {
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

            Username = Username.Trim();

            if (Username.Contains(" "))
            {
                await _notificationService.ShowToastAsync("Ім'я користувача не повинно містити пробілів.");
                return false;
            }

            if (!Regex.IsMatch(Username, "^[A-Za-z][A-Za-z0-9_]{2,}$"))
            {
                await _notificationService.ShowToastAsync("Ім'я користувача повинно починатися з літери, містити щонайменше 3 символи, з яких мінімум 2 — літери. Допустимі символи: англійські літери, цифри та нижнє підкреслення.");
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
            Email = Email.Trim();

            if (Email.Contains(" "))
            {
                await _notificationService.ShowToastAsync("Адреса електронної пошти не повинна містити пробілів.");
                return false;
            }

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