using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels
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
            await Shell.Current.GoToAsync("//TabBar");
        }
    }
} 