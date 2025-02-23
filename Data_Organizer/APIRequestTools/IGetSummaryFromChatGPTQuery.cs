using Data_Organizer_Core;
using Refit;

namespace Data_Organizer.APIRequestTools
{
    public interface IGetSummaryFromChatGPTQuery
    {
        [Post("/api/openai/summary")]
        Task<SummaryRequest> Execute([Body] SummaryRequest request);
    }
}
