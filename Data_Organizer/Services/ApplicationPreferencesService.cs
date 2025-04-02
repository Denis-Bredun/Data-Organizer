using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using Data_Organizer.MVVM.Models.Enums;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
using System.Linq;

namespace Data_Organizer.Services
{
    public class ApplicationPreferencesService : IApplicationPreferencesService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPreferenceService _preferenceService;
        private readonly IEnumDescriptionResolverService _enumDescriptionResolverService;
        private readonly MainPageViewModel _mainPageViewModel;
        private readonly HelpPageViewModel _helpPageViewModel;

        public ApplicationPreferencesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _preferenceService = _serviceProvider.GetRequiredService<IPreferenceService>();
            _enumDescriptionResolverService = _serviceProvider.GetRequiredService<IEnumDescriptionResolverService>();
            _mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();
            _helpPageViewModel = _serviceProvider.GetRequiredService<HelpPageViewModel>();
        }

        public void LoadPreferences()
        {
            LoadMainPagePreferences();
            LoadHelpPagePreferences();
        }

        public void SavePreferences()
        {
            SaveMainPagePreferences();
            SaveHelpPagePreferences();
        }

        private void LoadMainPagePreferences()
        {
            LoadFeaturePreference(
                AppEnums.Preferences.LastSelectedFeature,
                Features.Transcription);

            LoadLanguagePreference(
                AppEnums.Preferences.LastSelectedOutputLanguage,
                Languages.UA);

            LoadBoolPreference(
                AppEnums.Preferences.IsTextAddedAtTheEnd,
                true,
                value => _mainPageViewModel.IsTextAddedAtTheEnd = value);

            LoadBoolPreference(
                AppEnums.Preferences.HasVisitedMainPage,
                false,
                value => _mainPageViewModel.HasVisitedMainPage = value);
        }

        private void SaveMainPagePreferences()
        {
            SaveEnumPreference(
                AppEnums.Preferences.LastSelectedFeature,
                _mainPageViewModel.SelectedFeature?.FeatureType ?? Features.Transcription);

            SaveEnumPreference(
                AppEnums.Preferences.LastSelectedOutputLanguage,
                _mainPageViewModel.SelectedLanguage?.LanguageType ?? Languages.UA);

            SaveBoolPreference(
                AppEnums.Preferences.IsTextAddedAtTheEnd,
                _mainPageViewModel.IsTextAddedAtTheEnd);

            SaveBoolPreference(
                AppEnums.Preferences.HasVisitedMainPage,
                _mainPageViewModel.HasVisitedMainPage);
        }

        private void LoadHelpPagePreferences()
        {
            LoadBoolPreference(
                AppEnums.Preferences.HelpPopupHasBeenClosedOnce,
                false,
                value => _helpPageViewModel.HasBeenClosedOnce = value);
        }

        private void SaveHelpPagePreferences()
        {
            SaveBoolPreference(
                AppEnums.Preferences.HelpPopupHasBeenClosedOnce,
                _helpPageViewModel.HasBeenClosedOnce);
        }

        private void LoadFeaturePreference(
            AppEnums.Preferences preferenceKey,
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
            AppEnums.Preferences preferenceKey,
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
            AppEnums.Preferences preferenceKey,
            TEnum value) where TEnum : Enum
        {
            _preferenceService.SetPreference(preferenceKey, value);
        }

        private void LoadBoolPreference(
            AppEnums.Preferences preferenceKey,
            bool defaultValue,
            Action<bool> setterAction)
        {
            string storedValue = _preferenceService.GetPreference(preferenceKey, defaultValue);
            setterAction(bool.Parse(storedValue));
        }

        private void SaveBoolPreference(
            AppEnums.Preferences preferenceKey,
            bool value)
        {
            _preferenceService.SetPreference(preferenceKey, value);
        }
    }
}
