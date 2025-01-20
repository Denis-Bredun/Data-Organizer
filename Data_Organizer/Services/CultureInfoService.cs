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
                new LanguageModel { DisplayName = "UA", CultureCode = "uk-UA" },
                new LanguageModel { DisplayName = "EN", CultureCode = "en-US" },
                new LanguageModel { DisplayName = "RU", CultureCode = "ru-RU" }
            };
        }

        public CultureInfo GetCultureInfoFromLanguage(LanguageModel language)
        {
            return CultureInfo.GetCultureInfo(language.CultureCode);
        }
    }
}
