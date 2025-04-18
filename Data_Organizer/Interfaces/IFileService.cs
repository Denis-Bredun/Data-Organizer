using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IFileService
    {
        Task<bool> RequestPermissionsStorageReadAsync();
        Task<bool> RequestPermissionsStorageWriteAsync();
        Task<FileInfoModel> ImportTextAsync();
        Task ExportTextAsync(string text, Enums.TextFileFormat textFileFormat);
        Task<FileInfoModel> ImportAudiofileAsync(LanguageModel selectedLanguage);
    }
}
