using System.Text.Json.Serialization;

namespace Data_Organizer.MVVM.Models
{
    public class NoteHeader
    {
        public NoteHeader() { }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("noteBodyReferenceId")]
        public string NoteBodyReferenceId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("previewText")]
        public string PreviewText { get; set; }

        [JsonPropertyName("creationTime")]
        public DateTime CreationTime { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
