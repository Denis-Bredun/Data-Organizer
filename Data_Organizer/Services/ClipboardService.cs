using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class ClipboardService : IClipboardService
    {
        private readonly IClipboardWrapper _clipboard;

        public ClipboardService(IClipboardWrapper clipboardWrapper)
        {
            _clipboard = clipboardWrapper;
        }

        public async Task AddAsync(string text)
        {
            await _clipboard.SetTextAsync(text);
        }

        public async Task<string?> GetLastDataAsync()
        {
            var data = await _clipboard.GetTextAsync();
            return ConvertNullToString(data);
        }

        private string ConvertNullToString(string? data) => string.IsNullOrWhiteSpace(data) ? "" : data;
    }
}
