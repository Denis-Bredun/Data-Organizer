using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface IAudioTranscriptorService
    {
        event Action<string> OnTranscriptionUpdated;
        bool IsListening { get; }
        Task StartListeningAsync(CultureInfo cultureInfo);
        void StopListening();
    }
}
