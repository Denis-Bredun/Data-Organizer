using Data_Organizer.DTOs;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IOpenAIAPIRequestService
    {
        Task<SummaryRequestDTO?> GetSummaryAsync(string content, LanguageModel selectedLanguage);
    }
}
