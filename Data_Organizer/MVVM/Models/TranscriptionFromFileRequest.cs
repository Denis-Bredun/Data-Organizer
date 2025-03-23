using Microsoft.AspNetCore.Http;

namespace Data_Organizer.MVVM.Models
{
    public class TranscriptionFromFileRequest
    {
        public IFormFile AudioFile { get; set; }
        public string LanguageCode { get; set; } = "uk-UA";
    }
}
