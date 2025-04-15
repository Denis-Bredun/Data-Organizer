using Data_Organizer.DTOs;
using Refit;

namespace Data_Organizer.Queries
{
    public interface IOpenAIQueries
    {
        [Post("/openai/summary")]
        Task<SummaryRequestDTO> GetSummary([Body] SummaryRequestDTO request);
    }
}
