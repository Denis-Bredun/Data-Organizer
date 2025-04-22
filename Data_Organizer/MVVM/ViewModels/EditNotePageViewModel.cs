using CommunityToolkit.Mvvm.ComponentModel;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class EditNotePageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoading;
    }
}
