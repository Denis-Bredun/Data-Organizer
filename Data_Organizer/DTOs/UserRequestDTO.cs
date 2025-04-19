using Data_Organizer.MVVM.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class UserRequestDTO
    {
        [Required]
        [JsonPropertyName("userDTO")]
        public UserDTO UserDTO { get; set; }

        [JsonPropertyName("usersMetadataDTO")]
        public UsersMetadataDTO? UsersMetadataDTO { get; set; }

        [JsonPropertyName("creationDevice")]
        public DeviceInfoModel? CreationDevice { get; set; }

        [JsonPropertyName("deletionDevice")]
        public DeviceInfoModel? DeletionDevice { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
