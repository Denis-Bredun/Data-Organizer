using Data_Organizer.MVVM.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class AccountLoginRequestDTO
    {
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [Required]
        [JsonPropertyName("accountLogin")]
        public AccountLogin AccountLogin { get; set; }

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
