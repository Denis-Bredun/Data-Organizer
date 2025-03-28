﻿using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

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

        public async Task ExportTextAsync(string text, AppEnums.TextFileFormat textFileFormat)
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

        public async Task<string> ImportAudiofileAsync(LanguageModel selectedLanguage)
        {
            try
            {
                if (!await _fileService.RequestPermissionsStorageReadAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до читання не надано");
                    return null;
                }

                var transcription = await _fileService.ImportAudiofileAsync(selectedLanguage);

                if (transcription == null)
                {
                    await _notificationService.ShowToastAsync("Файл не обрано або помилка транскрипції");
                    return null;
                }

                await _notificationService.ShowToastAsync(
                    $"Успішно транскрибовано {transcription.Length} символів");
                return transcription;
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка транскрипції: {ex.Message}");
                return null;
            }
        }
    }

}
