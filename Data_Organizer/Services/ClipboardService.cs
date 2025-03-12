using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class ClipboardService : IClipboardService
    {
        public async Task AddAsync(string text)
        {
            await Clipboard.SetTextAsync(text);
        }

        public async Task<string?> GetLastDataAsync()
        {
            string? lastData = await Clipboard.GetTextAsync();
            return ConvertNullToString(lastData);
        }

        private string ConvertNullToString(string? data) => string.IsNullOrWhiteSpace(data) ? "" : data;
    }
}
