namespace Data_Organizer.Interfaces
{
    public interface IClipboardService
    {
        Task AddAsync(string text);
        Task<string?> GetLastDataAsync();
    }
}
