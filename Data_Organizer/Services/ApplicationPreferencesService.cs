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
        }

        private void SaveMainPagePreferences()
        {
            SaveLastSelectedFeature();
        }

        private void LoadLastSelectedFeature()
        {
            var key = AppEnums.Preferences.LastChoosenFeature;

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
            var key = AppEnums.Preferences.LastChoosenFeature;
            var value = _mainPageViewModel.SelectedFeature;

            _preferenceService.SetPreference(key, value);
        }
    }
}
