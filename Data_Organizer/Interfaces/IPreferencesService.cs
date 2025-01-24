namespace Data_Organizer.Interfaces
{
    public interface IPreferencesService
    {
        string GetPreference<T>(MVVM.Models.Enums.Preferences key, T? defaultValue);
        void SetPreference<T>(MVVM.Models.Enums.Preferences key, T value);
    }
}
