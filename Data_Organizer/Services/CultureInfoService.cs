using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Globalization;

namespace Data_Organizer.Services
{
    public class CultureInfoService : ICultureInfoService
    {
        public List<LanguageModel> Languages { get; }

        public CultureInfoService()
        {
            Languages = new List<LanguageModel>
        {
            new LanguageModel { DisplayName = "Українська", CultureCode = "uk-UA" },
            new LanguageModel { DisplayName = "Англійська", CultureCode = "en-US" },
            new LanguageModel { DisplayName = "Російська", CultureCode = "ru-RU" }
        };
        }

        public CultureInfo GetCultureInfoFromLanguage(LanguageModel language)
        {
            return CultureInfo.GetCultureInfo(language.CultureCode);
        }
    }
}
