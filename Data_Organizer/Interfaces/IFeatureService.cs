using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;

namespace Data_Organizer.Interfaces
{
    public interface IFeatureService
    {
        public ObservableCollection<FeatureModel> Features { get; }
        void AddSubscriptionMarkToFeatures();
        void RemoveSubscriptionMarkFromFeatures();
    }
}
