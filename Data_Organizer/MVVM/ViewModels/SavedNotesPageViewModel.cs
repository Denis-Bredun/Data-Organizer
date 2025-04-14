using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer_Server.Models;
using System.Collections.ObjectModel;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SavedNotesPageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoading;

        public ObservableCollection<NoteHeader> NoteHeaders { get; set; }

        public SavedNotesPageViewModel()
        {
            NoteHeaders = new ObservableCollection<NoteHeader>();
        }
    }
}
