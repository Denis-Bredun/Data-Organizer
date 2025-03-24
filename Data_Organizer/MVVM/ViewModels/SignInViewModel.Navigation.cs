using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels
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
            // добавить сохранение куда-то того, что метаданные не собираются
            IsLoading = true;
            await Shell.Current.GoToAsync("//TabBar");
        }
    }
} 