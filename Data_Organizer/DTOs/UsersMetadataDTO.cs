using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class UsersMetadataDTO
    {
        [JsonPropertyName("creationDate")]
        public DateTime? CreationDate { get; set; }

        [JsonPropertyName("creationDeviceId")]
        public string? CreationDeviceId { get; set; }

        [JsonPropertyName("creationLocation")]
        public Data_Organizer.Models.Location? CreationLocation { get; set; }

        [JsonPropertyName("deletionDate")]
        public DateTime? DeletionDate { get; set; }

        [JsonPropertyName("deletionDeviceId")]
        public string? DeletionDeviceId { get; set; }

        [JsonPropertyName("deletionLocation")]
        public Data_Organizer.Models.Location? DeletionLocation { get; set; }
    }
}
