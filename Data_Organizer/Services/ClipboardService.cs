using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class ClipboardService : IClipboardService
    {
        private readonly INotificationService _notificationService;

        public ClipboardService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Add(string text)
        {
            await Clipboard.SetTextAsync(text);
            await _notificationService.ShowToastAsync("Дані успішно скопійовані!");
        }

        public async Task<string?> GetLastData()
        {
            string? lastData = await Clipboard.GetTextAsync();
            return ConvertNullToString(lastData);
        }

        private string ConvertNullToString(string? data) => string.IsNullOrWhiteSpace(data) ? "" : data;
    }
}
