using Data_Organizer.MVVM.Models;
using Refit;

namespace Data_Organizer.APIRequestTools
{
    public interface IGetSummaryFromChatGPTQuery
    {
        [Post("/openai/summary")]
        Task<SummaryRequest> Execute([Body] SummaryRequest request);
    }
}
