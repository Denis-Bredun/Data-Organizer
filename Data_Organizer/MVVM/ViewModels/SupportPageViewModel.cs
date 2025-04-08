using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SupportPageViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        [ObservableProperty] private bool _isLoading;

        public SupportPageViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }

        [RelayCommand]
        public async Task GoBack()
        {
            IsLoading = true;

            if (_firebaseAuthService.IsUserAuthorized())
                await Shell.Current.GoToAsync("//SettingsPage");
            else
                await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}
