using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IFileService
    {
        Task<bool> RequestPermissionsStorageReadAsync();
        Task<bool> RequestPermissionsStorageWriteAsync();
        Task<string> ImportTextAsync();
        Task ExportTextAsync(string text, AppEnums.TextFileFormat textFileFormat);
        Task<string> ImportAudiofileAsync(LanguageModel selectedLanguage);
    }
}
