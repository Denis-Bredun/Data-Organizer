using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class WelcomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        [RelayCommand]
        public async Task NavigateToSignUpPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//SignUpPage");
        }

        [RelayCommand]
        public async Task NavigateToSignInPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
