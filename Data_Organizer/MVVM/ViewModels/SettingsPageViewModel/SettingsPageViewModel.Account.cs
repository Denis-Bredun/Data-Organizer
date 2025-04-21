using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SettingsPageViewModel
{
    public partial class SettingsPageViewModel
    {
        [RelayCommand]
        public async Task SignOut()
        {
            if (!await CheckInternetConnectionAsync())
                return;

            if (!await IsUserAuthorizedCheck())
                return;

            bool confirmed = await ShowSignOutConfirmation();

            if (!confirmed)
                return;

            IsLoading = true;

            if (IsMetadataStored)
                await _firestoreDbService.CreateAccountLogoutInstance();

            bool succeeded = await _firebaseAuthService.SignOut();

            if (succeeded)
            {
                _savedNotesPageViewModel.WasLoadedOnce = false;
                await Shell.Current.GoToAsync("//SignInPage");
            }
            else
                IsLoading = false;
        }

        [RelayCommand]
        public async Task DeleteAccount()
        {
            if (!await CheckInternetConnectionAsync())
                return;

            if (!await IsUserAuthorizedCheck())
                return;

            bool confirmed = await ShowDeletionConfirmation();
            if (!confirmed)
                return;

            IsLoading = true;

            Data_Organizer.MVVM.Models.Location location = await GetLocationForMetadata();
            if (location == null)
                return;

            await DeleteUserAccount(location);
        }

        private async Task<bool> IsUserAuthorizedCheck()
        {
            if (!_firebaseAuthService.IsUserAuthorized())
            {
                await _notificationService.ShowToastAsync("Користувач незареєстрований!");
                return false;
            }
            return true;
        }

        private async Task<bool> ShowDeletionConfirmation()
        {
            return await _notificationService.ShowConfirmationDialogAsync("Ви впевнені, що хочете видалити акаунт?\n"
                                                                        + "Його неможливо буде відновити!");
        }

        private async Task<bool> ShowSignOutConfirmation()
        {
            return await _notificationService.ShowConfirmationDialogAsync("Ви впевнені, що хочете вийти з акаунту?");
        }

        private async Task<Data_Organizer.MVVM.Models.Location> GetLocationForMetadata()
        {
            if (!IsMetadataStored)
                return null;

            var location = await _deviceServiceDecorator.GetCurrentLocationAsync();
            if (location == null)
            {
                IsLoading = false;
                await _notificationService.ShowToastAsync("Для збору метаданих потрібен доступ до вашої геолокації!");
            }
            return location;
        }

        private async Task DeleteUserAccount(Data_Organizer.MVVM.Models.Location location)
        {
            var uid = _firebaseAuthService.GetUid();
            await _firestoreDbService.RemoveUserAsync(uid, IsMetadataStored, location);

            bool succeeded = await _firebaseAuthService.DeleteAccount();

            if (succeeded)
            {
                _savedNotesPageViewModel.WasLoadedOnce = false;
                await Shell.Current.GoToAsync("//SignInPage");
            }
            else
            {
                IsLoading = false;
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
    }

}
