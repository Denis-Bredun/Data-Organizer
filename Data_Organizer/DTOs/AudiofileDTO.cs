using Microsoft.AspNetCore.Http;

namespace Data_Organizer.DTOs
{
    public class AudiofileDTO
    {
        public IFormFile AudioFile { get; set; }
        public string LanguageCode { get; set; } = "uk-UA";
    }
}
