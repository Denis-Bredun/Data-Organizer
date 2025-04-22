using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class EditNotePageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private NoteHeader _header;
        [ObservableProperty] private NoteBody _body;

        public EditNotePageViewModel()
        {
            Header = new NoteHeader();
            Body = new NoteBody();
        }
    }
}
