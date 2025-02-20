using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;

        [RelayCommand]
        public async Task NavigateToSignUpPage()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }

        [RelayCommand]
        public async Task NavigateToMainPage()
        {
            // добавить сохранение куда-то того, что метаданные не собираются

            await Shell.Current.GoToAsync("//TabBar");
        }

        [RelayCommand]
        public async Task SignIn()
        {
            // авторизация + подгрузка о том, чи собираем метаданные
        }
    }
}
