using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Organizer.DTOs
{
    public class NoteDTO
    {
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("noteBodyId")]
        public string? NoteBodyId { get; set; }

        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required]
        [JsonPropertyName("previewText")]
        public string PreviewText { get; set; }

        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [Required]
        [JsonPropertyName("creationTime")]
        public DateTime CreationTime { get; set; }

        [Required]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
