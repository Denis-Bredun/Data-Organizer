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
            IsLoading = true;

            bool succeeded = await _firebaseAuthService.DeleteAccount();

            if (succeeded)
                await Shell.Current.GoToAsync("//SignInPage");
            else
                IsLoading = false;
        }
    }

}
