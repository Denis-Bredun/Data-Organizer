using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.Services
{
    public class FileServiceDecorator : IFileServiceDecorator
    {
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;

        public FileServiceDecorator(
            IFileService fileService,
            INotificationService notificationService)
        {
            _fileService = fileService;
            _notificationService = notificationService;
        }

        public async Task<string> ImportTextAsync()
        {
            try
            {
                if (!await _fileService.RequestPermissionsStorageReadAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до читання не надано");
                    return null;
                }

                var content = await _fileService.ImportTextAsync();

                if (content == null)
                {
                    await _notificationService.ShowToastAsync("Файл не обрано");
                    return null;
                }

                await _notificationService.ShowToastAsync(
                    $"Успішно імпортовано {content.Length} символів");
                return content;
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка імпорту: {ex.Message}");
                return null;
            }
        }

        public async Task ExportTextAsync(string text, TextFileFormat textFileFormat)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    await _notificationService.ShowToastAsync("Немає тексту для експорту");
                    return;
                }

                if (!await _fileService.RequestPermissionsStorageWriteAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до запису не надано");
                    return;
                }

                var confirm = await _notificationService.ShowConfirmationDialogAsync("Зберегти файл?");
                if (!confirm) return;

                await _fileService.ExportTextAsync(text, textFileFormat);
                await _notificationService.ShowToastAsync(
                    $"Файл успішно збережено у форматі {textFileFormat}");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка експорту: {ex.Message}");
            }
        }
    }

}
