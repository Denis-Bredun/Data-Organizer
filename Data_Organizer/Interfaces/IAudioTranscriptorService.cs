using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface IAudioTranscriptorService
    {
        bool IsListening { get; }
        Task StartListening(CultureInfo cultureInfo);
        Task StopListening();
    }
}
