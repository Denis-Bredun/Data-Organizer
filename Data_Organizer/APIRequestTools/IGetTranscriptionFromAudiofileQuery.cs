using Data_Organizer.MVVM.Models;
using Refit;

namespace Data_Organizer.APIRequestTools
{
    public interface IGetTranscriptionFromAudiofileQuery
    {
        [Multipart]
        [Post("/api/azure/transcribe-file")]
        Task<TranscriptionResponse> Execute([AliasAs("AudioFile")] StreamPart audioFile, [AliasAs("LanguageCode")] string languageCode);
    }
}
