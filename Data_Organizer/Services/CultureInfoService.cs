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
                new LanguageModel("UA", "uk-UA", Enums.Languages.UA),
                new LanguageModel("EN", "en-US", Enums.Languages.EN),
                new LanguageModel("RU", "ru-RU", Enums.Languages.RU)
            };
        }

        public CultureInfo GetCultureInfoFromLanguage(LanguageModel language)
        {
            return CultureInfo.GetCultureInfo(language.CultureCode);
        }
    }
}
