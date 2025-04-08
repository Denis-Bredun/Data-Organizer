using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel
    {
        [RelayCommand]
        public async Task NavigateToSignInPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            // добавить сохранение куда-то того, что метаданные не собираются
            IsLoading = true;
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        public async Task NavigateToSupportPage()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync("//SupportPage");
        }
    }
}