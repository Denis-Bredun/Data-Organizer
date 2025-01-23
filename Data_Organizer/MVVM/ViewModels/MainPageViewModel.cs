using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        public IFeatureService FeatureService { get; private set; }
        public ICultureInfoService CultureInfoService { get; private set; }

        [ObservableProperty]
        private FeatureModel _selectedFeature;

        public MainPageViewModel(
            IFeatureService featureService,
            ICultureInfoService cultureInfoService)
        {
            FeatureService = featureService;
            _selectedFeature = FeatureService.Features[0]; // later I should save the last choosen feature to SETTINGS

            CultureInfoService = cultureInfoService;
        }
    }
}
