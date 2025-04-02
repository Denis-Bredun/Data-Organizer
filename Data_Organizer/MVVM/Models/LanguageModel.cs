using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.MVVM.Models
{
    public class LanguageModel
    {
        public string DisplayName { get; set; }
        public string CultureCode { get; set; }
        public Languages LanguageType { get; set; }

        public LanguageModel(string displayName, string cultureCode, Languages languageType)
        {
            DisplayName = displayName;
            CultureCode = cultureCode;
            LanguageType = languageType;
        }

        public override string ToString() => DisplayName;
    }
}
