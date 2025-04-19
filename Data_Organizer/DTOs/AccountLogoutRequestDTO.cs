using Data_Organizer.MVVM.Models;
using Data_Organizer_Server.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class AccountLogoutRequestDTO
    {
        [Required]
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [Required]
        [JsonPropertyName("accountLogoutDTO")]
        public AccountLogoutDTO AccountLogoutDTO { get; set; }

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
