using Google.Cloud.Firestore;

namespace Data_Organizer.Models
{
    public class AccountLogout
    {
        public DocumentReference UsersMetadata { get; set; }
        public DocumentReference Device { get; set; }
        public Location Location { get; set; }
        public DateTime Date { get; set; }
    }
}
