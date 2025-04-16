using Data_Organizer.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class ChangePasswordRequestDTO
    {
        [Required]
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = default!;

        [Required]
        [JsonPropertyName("changePassword")]
        public ChangePassword ChangePassword { get; set; } = default!;

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; } = default!;
    }
}
