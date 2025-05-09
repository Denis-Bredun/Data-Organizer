using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using Data_Organizer.Queries;

namespace Data_Organizer.Services
{
    public class OpenAIAPIRequestService : IOpenAIAPIRequestService
    {
        private readonly INotificationService _notificationService;
        private readonly IOpenAIQueries _openAIQueries;
        private readonly IFirebaseAuthService _firebaseAuthService;

        public OpenAIAPIRequestService(
            INotificationService notificationService,
            IOpenAIQueries getSummaryFromChatGPTQuery,
            IFirebaseAuthService firebaseAuthService)
        {
            _notificationService = notificationService;
            _openAIQueries = getSummaryFromChatGPTQuery;
            _firebaseAuthService = firebaseAuthService;
        }

        public async Task<SummaryRequestDTO?> GetSummaryAsync(string content, LanguageModel selectedLanguage)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
            {
                await _notificationService.ShowToastAsync("Користувач незареєстрований!");
                return null;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                await _notificationService.ShowToastAsync("Тези не можуть бути зроблені з пустоти!");
                return null;
            }

            content = $"Мова виводу: {selectedLanguage.CultureCode}\n\n{content}";

            var requestData = new SummaryRequestDTO
            {
                Content = content
            };

            SummaryRequestDTO? response = new SummaryRequestDTO();

            try
            {
                response = await _openAIQueries.GetSummary(requestData);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до серверу: {ex.Message}");
                return null;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до OpenAI: {response.Error}");
                return null;
            }

            return response;
        }
    }
}
