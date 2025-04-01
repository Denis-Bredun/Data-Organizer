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
                await NavigateToMainPage();
            else
                IsLoading = false;

            // добавить сохранение куда-то того, что метаданные собираются
        }

        [RelayCommand]
        public async Task ShowTipAboutMetadata()
        {
            await _notificationService.ShowToastAsync("Метадані включають в себе: геолокація, дата, час та" +
                                                      " пристрій реєстрації, авторизацій, виходів з акаунту" +
                                                      " та змінею паролю. Збір буде здійснюватись із метою " +
                                                      " можливості відслідковування активності акаунту та " +
                                                      "потенційних підозрюваних дій.",
                                                      17);
        }
    }
} 