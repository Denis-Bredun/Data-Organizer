using Data_Organizer.MVVM.Models;
using Refit;

namespace Data_Organizer.Queries
{
    public interface IAzureQueries
    {
        [Multipart]
        [Post("/azure/transcribe-file")]
        Task<TranscriptionResponse> GetTranscriptionFromAudiofile([AliasAs("AudioFile")] StreamPart audioFile, [AliasAs("LanguageCode")] string languageCode);
    }
}
