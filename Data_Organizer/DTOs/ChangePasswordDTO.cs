using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class ChangePasswordDTO
    {
        [JsonPropertyName("usersMetadataId")]
        public string? UsersMetadataId { get; set; }

        [Required]
        [JsonPropertyName("oldPassword")]
        public string OldPassword { get; set; }

        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

        [Required]
        [JsonPropertyName("location")]
        public Data_Organizer.MVVM.Models.Location Location { get; set; }

        [Required]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }

}
