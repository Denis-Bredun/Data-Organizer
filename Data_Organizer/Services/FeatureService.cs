using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;

namespace Data_Organizer.Services
{
    public class FeatureService : IFeatureService
    {
        public ObservableCollection<FeatureModel> Features { get; }

        public FeatureService()
        {
            Features = new ObservableCollection<FeatureModel>()
        {
            new FeatureModel("Транскрипція"),
            new FeatureModel("Конспект", true) // Здесь уже установлено значение для IsWithSubscription
        };
        }

        public void AddSubscriptionMarkToFeatures()
        {
            foreach (var feature in Features.Where(el => el.IsWithSubscription))
                feature.AddSubscriptionMark();
        }

        public void RemoveSubscriptionMarkFromFeatures()
        {
            foreach (var feature in Features.Where(el => el.IsWithSubscription))
                feature.RemoveSubscriptionMark();
        }
    }
}
