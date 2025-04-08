using Data_Organizer.Interfaces;
using Firebase.Auth;
using System.Text.Json;

namespace Data_Organizer.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly INotificationService _notificationService;
        private readonly FirebaseAuthClient _firebaseAuthClient;

        public event EventHandler AuthStateChanged;

        public FirebaseAuthService(
            INotificationService notificationService,
            FirebaseAuthClient firebaseAuthClient)
        {
            _notificationService = notificationService;
            _firebaseAuthClient = firebaseAuthClient;
        }

        public async Task<bool> SignUpAsync(string email, string password, string username = "")
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            try
            {
                await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(
                email,
                password,
                username);

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                if (errorMessage == "EMAIL_EXISTS")
                    await _notificationService.ShowToastAsync("Користувач з такою поштою вже існує!");
                else
                    await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            try
            {
                await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(
                email,
                password);

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                if (errorMessage == "INVALID_LOGIN_CREDENTIALS")
                    await _notificationService.ShowToastAsync("Неправильно введені пошта або пароль!");
                else if (errorMessage == "TOO_MANY_ATTEMPTS_TRY_LATER")
                    await _notificationService.ShowToastAsync("Дуже багато спроб... Спробуйте пізніше!");
                else if (errorMessage == "INVALID_EMAIL")
                    await _notificationService.ShowToastAsync("Неправильна пошта!");
                else
                    await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<bool> SignOut()
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            bool confirmed = await _notificationService.ShowConfirmationDialogAsync("Ви впевнені, що хочете вийти з акаунту?");

            if (!confirmed)
                return false;

            try
            {
                _firebaseAuthClient.SignOut();

                await _notificationService.ShowToastAsync("Ви успішно вийшли з акаунту!");

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<bool> SignOutWithoutNotification()
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            try
            {
                _firebaseAuthClient.SignOut();

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<bool> ResetPassword(string email)
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            try
            {
                await _firebaseAuthClient.ResetEmailPasswordAsync(email);

                await _notificationService.ShowToastAsync("Вам на пошту було відправлено посилання для зміни пароля!");

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                if (errorMessage == "INVALID_EMAIL")
                    await _notificationService.ShowToastAsync("Неправильна пошта!");
                else
                    await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<bool> DeleteAccount()
        {
            if (!await CheckInternetConnectionAsync())
                return false;

            if (!IsUserAuthorized())
            {
                await _notificationService.ShowToastAsync("Користувач незареєстрований!");
                return false;
            }

            bool confirmed = await _notificationService.ShowConfirmationDialogAsync("Ви впевнені, що хочете видалити акаунт?\n"
                                                                                    + "Його неможливо буде відновити!");

            if (!confirmed)
                return false;

            try
            {
                await _firebaseAuthClient.User.DeleteAsync();

                await _notificationService.ShowToastAsync("Акаунт було успішно видалено!");

                OnAuthStateChanged();

                return true;
            }
            catch (FirebaseAuthHttpException ex)
            {
                string errorMessage = GetErrorMessage(ex);

                if (errorMessage == "CREDENTIAL_TOO_OLD_LOGIN_AGAIN")
                    await _notificationService.ShowToastAsync("Ви давно авторизувались! Перезайдіть в акаунт.");
                else
                    await _notificationService.ShowToastAsync(errorMessage);

                return false;
            }
        }

        public async Task<string?> GetFreshToken()
        {
            return await _firebaseAuthClient?.User?.GetIdTokenAsync(true);
        }

        public bool IsUserAuthorized() => _firebaseAuthClient.User != null;

        public string GetUsername()
        {
            if (!IsUserAuthorized())
                return null;
            else
            {
                var displayName = _firebaseAuthClient.User?.Info?.DisplayName;

                return string.IsNullOrWhiteSpace(displayName) ? "Користувач" : displayName;
            }
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            var isConnectedToInternet = IsConnectedToInternet();

            if (!isConnectedToInternet)
                await _notificationService.ShowToastAsync("Немає Інтернет-підключення");

            return isConnectedToInternet;
        }

        private bool IsConnectedToInternet() => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        private string GetErrorMessage(FirebaseAuthHttpException ex)
        {
            string responseData = ex.ResponseData;
            using JsonDocument doc = JsonDocument.Parse(responseData);
            string errorMessage = doc.RootElement
                                  .GetProperty("error")
                                  .GetProperty("message")
                                  .GetString();

            return errorMessage;
        }

        private void OnAuthStateChanged()
        {
            AuthStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
