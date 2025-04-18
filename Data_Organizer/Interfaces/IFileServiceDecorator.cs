using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IFileServiceDecorator
    {
        Task<FileInfoModel> ImportTextAsync();
        Task ExportTextAsync(string text);
        Task<FileInfoModel> ImportAudiofileAsync();
    }
}
