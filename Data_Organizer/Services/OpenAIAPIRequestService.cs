using Data_Organizer.APIRequestTools;
using Data_Organizer.Interfaces;
using Data_Organizer_Core;

namespace Data_Organizer.Services
{
    public class OpenAIAPIRequestService : IOpenAIAPIRequestService
    {
        private readonly IGetSummaryFromChatGPTQuery _getSummaryFromChatGPTQuery;

        public OpenAIAPIRequestService(
            IGetSummaryFromChatGPTQuery getSummaryFromChatGPTQuery)
        {
            _getSummaryFromChatGPTQuery = getSummaryFromChatGPTQuery;
        }

        public async Task<SummaryRequest> GetSummaryAsync(string content)
        {
            var requestData = new SummaryRequest
            {
                Content = content
            };

            SummaryRequest response;

            try
            {
                response = await _getSummaryFromChatGPTQuery.Execute(requestData);

                if (!string.IsNullOrWhiteSpace(response.Error))
                    throw new Exception(response.Error);
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }
    }
}
