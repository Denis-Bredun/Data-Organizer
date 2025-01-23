using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;

namespace Data_Organizer.Services
{
    public class FeatureService : IFeatureService
    {
        private string _subscriptionFileName = "subscription.png";

        public ObservableCollection<FeatureModel> Features { get; }

        public FeatureService()
        {
            Features = new ObservableCollection<FeatureModel>()
            {
                new FeatureModel("Транскрипція"),
                new FeatureModel("Конспект", true, _subscriptionFileName)
            };
        }

        public void RemoveIconsForFeaturesInSubscription()
        {
            foreach (var feature in Features.Where(el => el.IsWithSubscription))
                feature.RemoveIcon();
        }

        public void SetIconsForFeaturesInSubscription()
        {
            foreach (var feature in Features.Where(el => el.IsWithSubscription))
                feature.SetIcon(_subscriptionFileName);
        }
    }
}
