using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Uid { get; set; }

        [FirestoreProperty]
        public DocumentReference? UsersMetadata { get; set; }

        [FirestoreProperty]
        public bool IsDeleted { get; set; }

        [FirestoreProperty]
        public bool IsMetadataStored { get; set; }
    }
}
