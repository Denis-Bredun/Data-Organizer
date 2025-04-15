using Google.Cloud.Firestore;

namespace Data_Organizer_Server.Models
{
    public class AccountLogout
    {
        public DocumentReference UsersMetadata { get; set; }
        public DocumentReference Device { get; set; }
        public Location Location { get; set; }
        public DateTime Date { get; set; }
    }
}
