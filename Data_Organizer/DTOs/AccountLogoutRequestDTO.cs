using Data_Organizer.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class AccountLogoutRequestDTO
    {
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = default!;

        [Required]
        [JsonPropertyName("accountLogout")]
        public AccountLogout AccountLogout { get; set; } = default!;

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; } = default!;
    }
}
