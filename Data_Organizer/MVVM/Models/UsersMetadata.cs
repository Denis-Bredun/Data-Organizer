using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    public class UsersMetadata
    {
        public DateTime? CreationDate { get; set; }
        public DocumentReference? CreationDevice { get; set; }
        public Location? CreationLocation { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DocumentReference? DeletionDevice { get; set; }
        public Location? DeletionLocation { get; set; }
    }
}
