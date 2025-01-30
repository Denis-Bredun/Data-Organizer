using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface IAudioTranscriptorService
    {
        event Action<string> OnTranscriptionUpdated;
        bool IsListening { get; }
        Task StartListening(CultureInfo cultureInfo);
        void StopListening();
    }
}
