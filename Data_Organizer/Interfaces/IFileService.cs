using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.Interfaces
{
    public interface IFileService
    {
        Task<bool> RequestPermissionsStorageReadAsync();
        Task<bool> RequestPermissionsStorageWriteAsync();
        Task<string> ImportTextAsync();
        Task ExportTextAsync(string text, TextFileFormat textFileFormat);
    }
}
