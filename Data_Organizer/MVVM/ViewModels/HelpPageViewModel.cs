using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class HelpPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        public HelpPageViewModel()
        {
        }

        [RelayCommand]
        public async Task CloseHelpAndNavigateToMainPage()
        {
            IsLoading = true;

            await Shell.Current.GoToAsync("//TabBar");
        }
    }
} 