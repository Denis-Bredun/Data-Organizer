using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IOpenAIAPIRequestService
    {
        Task<SummaryRequest?> GetSummaryAsync(string content);
    }
}
