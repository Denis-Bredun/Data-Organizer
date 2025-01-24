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
                new LanguageModel("UA", "uk-UA"),
                new LanguageModel("EN", "en-US"),
                new LanguageModel("RU", "ru-RU")
            };
        }

        public CultureInfo GetCultureInfoFromLanguage(LanguageModel language)
        {
            return CultureInfo.GetCultureInfo(language.CultureCode);
        }
    }
}
