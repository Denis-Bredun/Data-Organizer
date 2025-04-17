using Data_Organizer.Enums;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
using Data_Organizer.MVVM.ViewModels.SettingsPageViewModel;

namespace Data_Organizer.Services
{
    public class ApplicationPreferencesService : IApplicationPreferencesService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPreferenceService _preferenceService;
        private readonly IEnumDescriptionResolverService _enumDescriptionResolverService;
        private readonly MainPageViewModel _mainPageViewModel;
        private readonly HelpPageViewModel _helpPageViewModel;
        private readonly SettingsPageViewModel _settingsPageViewModel;

        public ApplicationPreferencesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _preferenceService = _serviceProvider.GetRequiredService<IPreferenceService>();
            _enumDescriptionResolverService = _serviceProvider.GetRequiredService<IEnumDescriptionResolverService>();
            _mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();
            _helpPageViewModel = _serviceProvider.GetRequiredService<HelpPageViewModel>();
            _settingsPageViewModel = _serviceProvider.GetRequiredService<SettingsPageViewModel>();
        }

        public void LoadPreferences()
        {
            LoadMainPagePreferences();
            LoadHelpPagePreferences();
            LoadSettingsPagePreferences();
        }

        public void SavePreferences()
        {
            SaveMainPagePreferences();
            SaveHelpPagePreferences();
            SaveSettingsPagePreferences();
        }

        private void LoadMainPagePreferences()
        {
            LoadFeaturePreference(
                Enums.Preferences.LastSelectedFeature,
                Features.Transcription);

            LoadLanguagePreference(
                Enums.Preferences.LastSelectedOutputLanguage,
                Languages.UA);

            LoadBoolPreference(
                Enums.Preferences.IsTextAddedAtTheEnd,
                true,
                value => _mainPageViewModel.IsTextAddedAtTheEnd = value);

            LoadBoolPreference(
                Enums.Preferences.HasVisitedMainPage,
                false,
                value => _mainPageViewModel.HasVisitedMainPage = value);
        }

        private void SaveMainPagePreferences()
        {
            SaveEnumPreference(
                Enums.Preferences.LastSelectedFeature,
                _mainPageViewModel.SelectedFeature?.FeatureType ?? Features.Transcription);

            SaveEnumPreference(
                Enums.Preferences.LastSelectedOutputLanguage,
                _mainPageViewModel.SelectedLanguage?.LanguageType ?? Languages.UA);

            SaveBoolPreference(
                Enums.Preferences.IsTextAddedAtTheEnd,
                _mainPageViewModel.IsTextAddedAtTheEnd);

            SaveBoolPreference(
                Enums.Preferences.HasVisitedMainPage,
                _mainPageViewModel.HasVisitedMainPage);
        }

        private void LoadHelpPagePreferences()
        {
            LoadBoolPreference(
                Enums.Preferences.HelpPopupHasBeenClosedOnce,
                false,
                value => _helpPageViewModel.HasBeenClosedOnce = value);
        }

        private void SaveHelpPagePreferences()
        {
            SaveBoolPreference(
                Enums.Preferences.HelpPopupHasBeenClosedOnce,
                _helpPageViewModel.HasBeenClosedOnce);
        }

        private void LoadFeaturePreference(
            Enums.Preferences preferenceKey,
            Features defaultFeature)
        {
            string storedValue = _preferenceService.GetPreference(preferenceKey, defaultFeature);
            Features selectedFeature;

            try
            {
                selectedFeature = Enum.Parse<Features>(storedValue);
            }
            catch
            {
                selectedFeature = defaultFeature;
            }

            _mainPageViewModel.SelectedFeature = _mainPageViewModel.Features
                .FirstOrDefault(f => f.FeatureType == selectedFeature)
                ?? _mainPageViewModel.Features.FirstOrDefault();
        }

        private void LoadLanguagePreference(
            Enums.Preferences preferenceKey,
            Languages defaultLanguage)
        {
            string storedValue = _preferenceService.GetPreference(preferenceKey, defaultLanguage);
            Languages selectedLanguage;

            try
            {
                selectedLanguage = Enum.Parse<Languages>(storedValue);
            }
            catch
            {
                selectedLanguage = defaultLanguage;
            }

            _mainPageViewModel.SelectedLanguage = _mainPageViewModel.CultureInfoService.Languages
                .FirstOrDefault(l => l.LanguageType == selectedLanguage)
                ?? _mainPageViewModel.CultureInfoService.Languages.FirstOrDefault();
        }

        private void SaveEnumPreference<TEnum>(
            Enums.Preferences preferenceKey,
            TEnum value) where TEnum : Enum
        {
            _preferenceService.SetPreference(preferenceKey, value);
        }

        private void LoadBoolPreference(
            Enums.Preferences preferenceKey,
            bool defaultValue,
            Action<bool> setterAction)
        {
            string storedValue = _preferenceService.GetPreference(preferenceKey, defaultValue);
            setterAction(bool.Parse(storedValue));
        }

        private void SaveBoolPreference(
            Enums.Preferences preferenceKey,
            bool value)
        {
            _preferenceService.SetPreference(preferenceKey, value);
        }

        private void LoadSettingsPagePreferences()
        {
            LoadBoolPreference(
                Enums.Preferences.IsMetadataStored,
                false,
                value => _settingsPageViewModel.ChangeMetadataFlagWithoutAsking(value));
        }

        private void SaveSettingsPagePreferences()
        {
            SaveBoolPreference(
                Enums.Preferences.IsMetadataStored,
                _settingsPageViewModel.IsMetadataStored);
        }
    }
}
