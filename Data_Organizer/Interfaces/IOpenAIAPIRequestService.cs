using Data_Organizer_Core;

namespace Data_Organizer.Interfaces
{
    public interface IOpenAIAPIRequestService
    {
        Task<SummaryRequest> GetSummaryAsync(string content);
    }
}
