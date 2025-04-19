using Google.Cloud.Firestore;

namespace Data_Organizer.MVVM.Models
{
    public class User
    {
        public string Uid { get; set; }
        public DocumentReference? UsersMetadata { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsMetadataStored { get; set; }
    }
}
