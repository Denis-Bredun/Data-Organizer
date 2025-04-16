using CommunityToolkit.Mvvm.Input;

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

            Data_Organizer.Models.Location location = null;

            if (IsMetadataStored)
            {
                location = await _deviceServiceDecorator.GetCurrentLocationAsync();

                if (location == null)
                {
                    IsLoading = false;
                    await _notificationService.ShowToastAsync("Для збору метаданих потрібен доступ до вашої геолокації!");
                    return;
                }
            }

            bool succeeded = await _firebaseAuthService.SignUpAsync(Email, Password, Username);

            if (succeeded)
            {
                await _firestoreDbService.CreateUserAsync(IsMetadataStored, location);

                CleanFields();

                await NavigateToMainPage();
            }
            else
                IsLoading = false;
        }
    }
}