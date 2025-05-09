using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Services
{
    public class FileServiceDecorator : IFileServiceDecorator
    {
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly ICultureInfoService _cultureInfoService;

        public FileServiceDecorator(
            IFileService fileService,
            INotificationService notificationService,
            IFirebaseAuthService firebaseAuthService,
            ICultureInfoService cultureInfoService)
        {
            _fileService = fileService;
            _notificationService = notificationService;
            _firebaseAuthService = firebaseAuthService;
            _cultureInfoService = cultureInfoService;
        }

        public async Task<FileInfoModel> ImportTextAsync()
        {
            if (!await CheckIfUserIsAuthorized())
                return null;

            try
            {
                if (!await _fileService.RequestPermissionsStorageReadAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до читання не надано");
                    return null;
                }

                var fileInfoObj = await _fileService.ImportTextAsync();

                if (fileInfoObj == null)
                {
                    await _notificationService.ShowToastAsync("Файл не обрано");
                    return null;
                }

                await _notificationService.ShowToastAsync(
                    $"Успішно імпортовано {fileInfoObj.Content.Length} символів");
                return fileInfoObj;
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка імпорту: {ex.Message}");
                return null;
            }
        }

        public async Task ExportTextAsync(string text)
        {
            if (!await CheckIfUserIsAuthorized())
                return;

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

                const string
                    pdf = "PDF",
                    docx = "DOCX",
                    txt = "TXT";

                var answer = await _notificationService.ShowActionSheetAsync("В якому розширенні бажаєте експортувати?", pdf, docx, txt);

                Enums.TextFileFormat format;
                switch (answer)
                {
                    case pdf:
                        format = Enums.TextFileFormat.PDF;
                        break;
                    case docx:
                        format = Enums.TextFileFormat.DOCX;
                        break;
                    case txt:
                        format = Enums.TextFileFormat.TXT;
                        break;
                    default:
                        return;
                }

                var confirm = await _notificationService.ShowConfirmationDialogAsync("Зберегти файл?");
                if (!confirm) return;

                await _fileService.ExportTextAsync(text, format);
                await _notificationService.ShowToastAsync(
                    $"Файл успішно збережено у форматі {format}");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка експорту: {ex.Message}");
            }
        }

        public async Task<FileInfoModel> ImportAudiofileAsync()
        {
            if (!await CheckIfUserIsAuthorized())
                return null;

            if (!await CheckInternetConnectionAsync())
                return null;

            try
            {
                if (!await _fileService.RequestPermissionsStorageReadAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до читання не надано");
                    return null;
                }

                var languageOptions = _cultureInfoService.Languages.Select(l => l.DisplayName).ToArray();
                var answer = await _notificationService.ShowActionSheetAsync("Яка мова в аудіофайлі?", languageOptions);

                if (answer == "Нічого" || string.IsNullOrEmpty(answer))
                    return null;

                var selectedLanguage = _cultureInfoService.Languages.FirstOrDefault(l => l.DisplayName == answer);
                var fileInfoObj = await _fileService.ImportAudiofileAsync(selectedLanguage);

                if (fileInfoObj == null)
                {
                    await _notificationService.ShowToastAsync("Файл не обрано або помилка транскрипції");
                    return null;
                }

                await _notificationService.ShowToastAsync(
                    $"Успішно транскрибовано {fileInfoObj.Content.Length} символів");
                return fileInfoObj;
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка транскрипції: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> CheckIfUserIsAuthorized()
        {
            bool isAuthorized = _firebaseAuthService.IsUserAuthorized();

            if (!isAuthorized)
                await _notificationService.ShowToastAsync("Користувач незареєстрований!");

            return isAuthorized;
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            var isConnectedToInternet = IsConnectedToInternet();

            if (!isConnectedToInternet)
                await _notificationService.ShowToastAsync("Немає Інтернет-підключення");

            return isConnectedToInternet;
        }

        private bool IsConnectedToInternet() => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
