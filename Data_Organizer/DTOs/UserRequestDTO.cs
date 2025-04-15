using Data_Organizer_Server.Models;

namespace Data_Organizer.DTOs
{
    public class UserRequestDTO
    {
        public User User { get; set; }
        public UsersMetadata UsersMetadata { get; set; }
        public string Error { get; internal set; }
    }
}
