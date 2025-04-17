using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class UserMetadataFlagUpdateDTO
    {
        [Required]
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = default!;

        [JsonPropertyName("isMetadataStored")]
        public bool IsMetadataStored { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
