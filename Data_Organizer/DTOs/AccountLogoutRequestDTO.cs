using Data_Organizer.MVVM.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class AccountLogoutRequestDTO
    {
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [Required]
        [JsonPropertyName("accountLogout")]
        public AccountLogout AccountLogout { get; set; }

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
