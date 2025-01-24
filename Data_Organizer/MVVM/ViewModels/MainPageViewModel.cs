using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IPreferencesService _preferencesService;

        [ObservableProperty]
        private FeatureModel _selectedFeature;

        public IFeatureService FeatureService { get; }
        public ICultureInfoService CultureInfoService { get; }

        public MainPageViewModel(
            IPreferencesService preferencesService,
            IFeatureService featureService,
            ICultureInfoService cultureInfoService)
        {
            _preferencesService = preferencesService;
            FeatureService = featureService;
            _selectedFeature = FeatureService.Features[0]; // later I should save the last choosen feature to SETTINGS

            CultureInfoService = cultureInfoService;
        }
    }
}
