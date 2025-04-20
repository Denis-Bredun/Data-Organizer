using Google.Cloud.Firestore;

namespace Data_Organizer.MVVM.Models
{
    public class NoteHeader
    {
        public string UserId { get; set; }
        public DocumentReference NoteBodyReference { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
