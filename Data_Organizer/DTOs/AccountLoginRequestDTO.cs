using Data_Organizer.MVVM.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class AccountLoginRequestDTO
    {
        [Required]
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [Required]
        [JsonPropertyName("accountLoginDTO")]
        public AccountLoginDTO AccountLoginDTO { get; set; }

        [Required]
        [JsonPropertyName("deviceInfo")]
        public DeviceInfoModel DeviceInfo { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
