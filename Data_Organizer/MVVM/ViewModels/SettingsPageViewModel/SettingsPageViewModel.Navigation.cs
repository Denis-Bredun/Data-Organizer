using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SettingsPageViewModel
{
    public partial class SettingsPageViewModel
    {
        [RelayCommand]
        public async Task NavigateToSignInPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        public async Task NavigateToResetPasswordPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//ResetPasswordPage");
        }

        [RelayCommand]
        public async Task NavigateToSupportPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SupportPage");
        }
    }

}
