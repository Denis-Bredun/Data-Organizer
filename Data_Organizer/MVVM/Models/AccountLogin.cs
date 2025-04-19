using Google.Cloud.Firestore;

namespace Data_Organizer.MVVM.Models
{
    public class AccountLogin
    {
        public DocumentReference UsersMetadata { get; set; }
        public DocumentReference Device { get; set; }
        public Location Location { get; set; }
        public DateTime Date { get; set; }
    }
}
