using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class MauiClipboardWrapper : IClipboardWrapper
    {
        public async Task SetTextAsync(string text) => await Clipboard.SetTextAsync(text);
        public async Task<string?> GetTextAsync() => await Clipboard.GetTextAsync();
    }
}
