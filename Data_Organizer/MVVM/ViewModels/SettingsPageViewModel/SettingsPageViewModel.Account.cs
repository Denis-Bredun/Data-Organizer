using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SettingsPageViewModel
{
    public partial class SettingsPageViewModel
    {
        [RelayCommand]
        public async Task SignOut()
        {
            IsLoading = true;

            bool succeeded = await _firebaseAuthService.SignOut();

            if (succeeded)
                await Shell.Current.GoToAsync("//SignInPage");
            else
                IsLoading = false;
        }

        [RelayCommand]
        public async Task DeleteAccount()
        {          
            if (!await CheckInternetConnectionAsync())
                return;

            if (!_firebaseAuthService.IsUserAuthorized())
            {
                await _notificationService.ShowToastAsync("Користувач незареєстрований!");
                return;
            }

            bool confirmed = await _notificationService.ShowConfirmationDialogAsync("Ви впевнені, що хочете видалити акаунт?\n"
                                                                                    + "Його неможливо буде відновити!");

            if (!confirmed)
                return;

            IsLoading = true;

            Data_Organizer.Models.Location location = null;

            if (IsMetadataStored)
            {
                location = await _deviceServiceDecorator.GetCurrentLocationAsync();

                if (location == null)
                {
                    IsLoading = false;
                    await _notificationService.ShowToastAsync("Для збору метаданих потрібен доступ до вашої геолокації!");
                    return;
                }
            }

            var uid = _firebaseAuthService.GetUid();
            await _firestoreDbService.RemoveUserAsync(uid, IsMetadataStored, location);

            bool succeeded = await _firebaseAuthService.DeleteAccount();

            if (succeeded)
            {
                await Shell.Current.GoToAsync("//SignInPage");
            }
            else
                IsLoading = false;
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            var isConnectedToInternet = IsConnectedToInternet();

            if (!isConnectedToInternet)
                await _notificationService.ShowToastAsync("Немає Інтернет-підключення");

            return isConnectedToInternet;
        }

        private bool IsConnectedToInternet() => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }

}
