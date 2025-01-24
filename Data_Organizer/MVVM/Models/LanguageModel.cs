namespace Data_Organizer.MVVM.Models
{
    public class LanguageModel
    {
        public string DisplayName { get; set; }
        public string CultureCode { get; set; }

        public LanguageModel(string displayName, string cultureCode)
        {
            DisplayName = displayName;
            CultureCode = cultureCode;
        }

        public override string ToString() => DisplayName;
    }
}
