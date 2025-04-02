using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.MVVM.Models.Enums;

namespace Data_Organizer.MVVM.Models
{
    public partial class FeatureModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;
        
        public Features FeatureType { get; }

        public FeatureModel(string title, Features featureType)
        {
            Title = title;
            FeatureType = featureType;
        }

        public override string ToString() => Title;
    }
}
