namespace Data_Organizer_Server.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GetSummary(string content);
    }
}
