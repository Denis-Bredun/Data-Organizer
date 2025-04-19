using Data_Organizer.MVVM.Models;
using Refit;

namespace Data_Organizer.Queries
{
    public interface ITranskriptorQueries
    {
        [Multipart]
        [Post("/transkriptor/transcribe-file")]
        Task<TranscriptionResponse> GetTranscriptionFromAudiofile([AliasAs("AudioFile")] StreamPart audioFile, [AliasAs("LanguageCode")] string languageCode);
    }
}
