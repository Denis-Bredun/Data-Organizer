namespace Data_Organizer.Interfaces
{
    public interface IPreferenceService
    {
        string GetPreference<T>(Enums.Preferences key, T? defaultValue);
        void SetPreference<T>(Enums.Preferences key, T value);
    }
}
