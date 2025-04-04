using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels.SignUpViewModel
{
    public partial class SignUpViewModel
    {
        [RelayCommand]
        public async Task SignUp()
        {
            if (!await ValidateData())
                return;

            IsLoading = true;

            bool succeeded = await _firebaseAuthService.SignUpAsync(Email, Password, Username);

            if (succeeded)
            {
                CleanFields();
                await NavigateToMainPage();
            }
            else
                IsLoading = false;

            // добавить сохранение куда-то того, что метаданные собираются
        }
    }
} 