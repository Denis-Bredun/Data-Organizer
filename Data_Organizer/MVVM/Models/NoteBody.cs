using CommunityToolkit.Mvvm.ComponentModel;

namespace Data_Organizer.MVVM.Models
{
    public partial class NoteBody : ObservableObject
    {
        [ObservableProperty]
        private string content;
    }
}
