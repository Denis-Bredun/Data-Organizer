namespace Data_Organizer.Interfaces
{
    public interface IClipboardService
    {
        Task Add(string text);
        Task<string?> GetLastData();
    }
}
