using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class ClipboardService : IClipboardService
    {
        public async Task Add(string text)
        {
            await Clipboard.SetTextAsync(text);
        }

        public async Task<string?> GetLastData()
        {
            string? lastData = await Clipboard.GetTextAsync();
            return ConvertNullToString(lastData);
        }

        private string ConvertNullToString(string? data) => string.IsNullOrWhiteSpace(data) ? "" : data;
    }
}
