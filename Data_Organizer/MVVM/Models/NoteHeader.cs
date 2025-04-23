using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace Data_Organizer.MVVM.Models
{
    public partial class NoteHeader : ObservableObject
    {
        public NoteHeader() { }

        [ObservableProperty]
        [JsonPropertyName("userId")]
        private string userId;

        [ObservableProperty]
        [JsonPropertyName("noteBodyReferenceId")]
        private string noteBodyReferenceId;

        [ObservableProperty]
        [JsonPropertyName("title")]
        private string title;

        [ObservableProperty]
        [JsonPropertyName("previewText")]
        private string previewText;

        [ObservableProperty]
        [JsonPropertyName("creationTime")]
        private DateTime creationTime;

        [ObservableProperty]
        [JsonPropertyName("isDeleted")]
        private bool isDeleted;
    }
}
