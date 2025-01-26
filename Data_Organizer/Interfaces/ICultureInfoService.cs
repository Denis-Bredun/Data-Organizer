using Data_Organizer.MVVM.Models;
using System.Globalization;

namespace Data_Organizer.Interfaces
{
    public interface ICultureInfoService
    {
        public List<LanguageModel> Languages { get; }
        CultureInfo GetCultureInfoFromLanguage(LanguageModel language);
    }
}
