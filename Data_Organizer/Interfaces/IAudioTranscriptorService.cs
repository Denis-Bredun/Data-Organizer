using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface IAudioTranscriptorService
    {
        Task StartListening(CultureInfo cultureInfo);
        Task StopListening();
    }
}
