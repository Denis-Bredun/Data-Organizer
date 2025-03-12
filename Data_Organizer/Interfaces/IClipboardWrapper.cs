namespace Data_Organizer.Interfaces
{
    public interface IClipboardWrapper
    {
        Task SetTextAsync(string text);
        Task<string?> GetTextAsync();
    }
}
