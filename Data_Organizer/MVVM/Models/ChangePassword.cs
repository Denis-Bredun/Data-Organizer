using Google.Cloud.Firestore;

namespace Data_Organizer.Models
{
    public class ChangePassword
    {
        public DocumentReference UsersMetadata { get; set; }
        public string Hashcode { get; set; }
        public DocumentReference Device { get; set; }
        public Location Location { get; set; }
        public DateTime Date { get; set; }
    }
}
