using Firebase.Auth;

namespace Data_Organizer.MVVM.Models
{
    public class UserData
    {
        public UserInfo UserInfo { get; set; }
        public FirebaseCredential Credential { get; set; }
    }
}
