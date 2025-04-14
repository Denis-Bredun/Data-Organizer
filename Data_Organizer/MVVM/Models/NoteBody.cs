using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    [FirestoreData]
    public class NoteBody
    {
        [FirestoreProperty]
        public string Content { get; set; }
    }
}
