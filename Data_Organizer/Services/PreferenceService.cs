using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class PreferenceService : IPreferenceService
    {
        public string GetPreference<T>(Enums.Preferences key, T? defaultValue = default)
        {
            var strKey = key.ToString();
            var strDefaultValue = defaultValue?.ToString();

            var preferenceValue = Microsoft.Maui.Storage.Preferences.Get(strKey, strDefaultValue);

            return preferenceValue;
        }

        public void SetPreference<T>(Enums.Preferences key, T value)
        {
            var strKey = key.ToString();
            var strValue = value.ToString();

            Microsoft.Maui.Storage.Preferences.Set(strKey, strValue);
        }
    }
}
