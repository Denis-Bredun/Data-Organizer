using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    [QueryProperty(nameof(ReturnTo), "returnTo")]
    public partial class SupportPageViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        [ObservableProperty] private bool _isLoading;

        public string ReturnTo { get; set; }

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
            {
                if (!string.IsNullOrEmpty(ReturnTo))
                    await Shell.Current.GoToAsync(ReturnTo);
                else
                    await Shell.Current.GoToAsync("//SignInPage");
            }
        }
    }
}
