using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IEnumDescriptionResolverService _enumDescriptionResolverService;

        [ObservableProperty]
        private FeatureModel _selectedFeature;

        public IFeatureService FeatureService { get; }
        public ICultureInfoService CultureInfoService { get; }

        public MainPageViewModel(
            IPreferenceService preferenceService,
            IEnumDescriptionResolverService enumDescriptionResolverService,
            IFeatureService featureService,
            ICultureInfoService cultureInfoService)
        {
            _preferenceService = preferenceService;
            _enumDescriptionResolverService = enumDescriptionResolverService;

            FeatureService = featureService;
            CultureInfoService = cultureInfoService;

            LoadPreferences();
        }

        private void LoadPreferences()
        {
            LoadLastSelectedFeature();
            //... loading of other preferences
        }

        private void LoadLastSelectedFeature()
        {
            var key = Models.Enums.Preferences.LastChoosenFeature;

            var defaultValueEnum = Features.Transcription;
            var defaultValueEnumDescription = _enumDescriptionResolverService.GetEnumDescription(defaultValueEnum);
            var defaultValue = FeatureService.Features.FirstOrDefault(
                    f => f.Title.Contains(defaultValueEnumDescription));

            string lastSelectedFeature = _preferenceService.GetPreference(
                key, defaultValue);

            SelectedFeature = FeatureService.Features.FirstOrDefault(
                    f => f.Title.Contains(lastSelectedFeature));
        }
    }
}
