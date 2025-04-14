using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    [FirestoreData]
    public class ChangePassword
    {
        [FirestoreProperty]
        public DocumentReference UsersMetadata { get; set; }

        [FirestoreProperty]
        public string Hashcode { get; set; }

        [FirestoreProperty]
        public DocumentReference Device { get; set; }

        [FirestoreProperty]
        public Location Location { get; set; }

        [FirestoreProperty]
        public DateTime Date { get; set; }
    }
}
