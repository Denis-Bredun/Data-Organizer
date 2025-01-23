using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels
{
    public class MainPageViewModel
    {
        private readonly IFeatureService _featureService;
        private readonly ICultureInfoService _cultureInfoService;

        public MainPageViewModel(
            IFeatureService featureService,
            ICultureInfoService cultureInfoService)
        {
            _featureService = featureService;
            _cultureInfoService = cultureInfoService;
        }
    }
}
