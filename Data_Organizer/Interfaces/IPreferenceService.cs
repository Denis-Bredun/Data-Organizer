namespace Data_Organizer.Interfaces
{
    public interface IPreferenceService
    {
        string GetPreference<T>(AppEnums.Preferences key, T? defaultValue);
        void SetPreference<T>(AppEnums.Preferences key, T value);
    }
}
