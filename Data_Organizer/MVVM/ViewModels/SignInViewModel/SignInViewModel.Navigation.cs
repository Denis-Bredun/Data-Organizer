using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SignInViewModel
{
    public partial class SignInViewModel
    {
        [RelayCommand]
        public async Task NavigateToSignUpPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SignUpPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//MainPage");
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