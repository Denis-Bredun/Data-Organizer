using Data_Organizer.APIRequestTools;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Services
{
    public class OpenAIAPIRequestService : IOpenAIAPIRequestService
    {
        private readonly INotificationService _notificationService;
        private readonly IGetSummaryFromChatGPTQuery _getSummaryFromChatGPTQuery;
        private readonly IFirebaseAuthService _firebaseAuthService;

        public OpenAIAPIRequestService(
            INotificationService notificationService,
            IGetSummaryFromChatGPTQuery getSummaryFromChatGPTQuery,
            IFirebaseAuthService firebaseAuthService)
        {
            _notificationService = notificationService;
            _getSummaryFromChatGPTQuery = getSummaryFromChatGPTQuery;
            _firebaseAuthService = firebaseAuthService;
        }

        public async Task<SummaryRequest?> GetSummaryAsync(string content, LanguageModel selectedLanguage)
        {
            content = $"Мова виводу: {selectedLanguage.CultureCode}\n\n{content}";

            var requestData = new SummaryRequest
            {
                Content = content
            };

            SummaryRequest response = await _getSummaryFromChatGPTQuery.Execute(requestData);

            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                await _notificationService.ShowToastAsync($"Ошибка при запросе к OpenAI: {response.Error}");
                return null;
            }

            return response;
        }
    }
}
