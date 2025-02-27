using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.Services
{
    public class ApplicationPreferencesService : IApplicationPreferencesService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPreferenceService _preferenceService;
        private readonly IEnumDescriptionResolverService _enumDescriptionResolverService;
        private readonly MainPageViewModel _mainPageViewModel;

        public ApplicationPreferencesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _preferenceService = _serviceProvider.GetRequiredService<IPreferenceService>(); ;
            _enumDescriptionResolverService = _serviceProvider.GetRequiredService<IEnumDescriptionResolverService>(); ;
            _mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();
        }

        public void LoadPreferences()
        {
            LoadMainPagePreferences();
        }

        public void SavePreferences()
        {
            SaveMainPagePreferences();
        }

        private void LoadMainPagePreferences()
        {
            LoadLastSelectedFeature();
            LoadLastSelectedOutputLanguage();
            LoadPreferenceIsTextAddedAtTheEnd();
        }

        private void SaveMainPagePreferences()
        {
            SaveLastSelectedFeature();
            SaveLastSelectedOutputLanguage();
            SavePreferenceIsTextAddedAtTheEnd();
        }

        private void LoadLastSelectedFeature()
        {
            var key = AppEnums.Preferences.LastSelectedFeature;

            var defaultValueEnum = AppEnums.Features.Transcription;
            var defaultValueEnumDescription = _enumDescriptionResolverService.GetEnumDescription(defaultValueEnum);
            var defaultValue = _mainPageViewModel.FeatureService.Features.FirstOrDefault(
                    f => f.Title.Contains(defaultValueEnumDescription));

            string lastSelectedFeature = _preferenceService.GetPreference(
                key, defaultValue);

            _mainPageViewModel.SelectedFeature = _mainPageViewModel.FeatureService.Features.FirstOrDefault(
                    f => f.Title.Contains(lastSelectedFeature));
        }

        private void SaveLastSelectedFeature()
        {
            var key = AppEnums.Preferences.LastSelectedFeature;
            var value = _mainPageViewModel.SelectedFeature;

            _preferenceService.SetPreference(key, value);
        }

        private void LoadLastSelectedOutputLanguage()
        {
            var key = AppEnums.Preferences.LastSelectedOutputLanguage;

            var defaultValueEnum = AppEnums.Languages.UA;
            var defaultValueEnumDescription = defaultValueEnum.ToString();
            var defaultValue = _mainPageViewModel.CultureInfoService.Languages.FirstOrDefault(
                    l => l.DisplayName.Contains(defaultValueEnumDescription));

            string lastSelectedOutputLanguage = _preferenceService.GetPreference(
                key, defaultValue);

            _mainPageViewModel.SelectedLanguage = _mainPageViewModel.CultureInfoService.Languages.FirstOrDefault(
                    l => l.DisplayName.Contains(lastSelectedOutputLanguage));
        }

        private void SaveLastSelectedOutputLanguage()
        {
            var key = AppEnums.Preferences.LastSelectedOutputLanguage;
            var value = _mainPageViewModel.SelectedLanguage;

            _preferenceService.SetPreference(key, value);
        }

        private void LoadPreferenceIsTextAddedAtTheEnd()
        {
            var key = AppEnums.Preferences.IsTextAddedAtTheEnd;

            var defaultValue = true;

            string isTextAddedAtTheEnd = _preferenceService.GetPreference(key, defaultValue);

            _mainPageViewModel.IsTextAddedAtTheEnd = bool.Parse(isTextAddedAtTheEnd);
        }

        private void SavePreferenceIsTextAddedAtTheEnd()
        {
            var key = AppEnums.Preferences.IsTextAddedAtTheEnd;
            var value = _mainPageViewModel.IsTextAddedAtTheEnd;

            _preferenceService.SetPreference(key, value);
        }
    }
}
