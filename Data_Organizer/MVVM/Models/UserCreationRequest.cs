namespace Data_Organizer_Server.Models
{
    public class UserCreationRequest
    {
        public User User { get; set; }
        public UsersMetadata UsersMetadata { get; set; }
        public string Error { get; internal set; }
    }
}
