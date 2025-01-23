using CommunityToolkit.Mvvm.ComponentModel;

namespace Data_Organizer.MVVM.Models
{
    public partial class FeatureModel : ObservableObject
    {
        private const string SUBSCRIPTION_MARK = " ✖";
        private readonly string _baseTitle;

        [ObservableProperty]
        public string _title;

        public bool IsWithSubscription { get; }

        public FeatureModel(string baseTitle, bool isWithSubscription = false)
        {
            _baseTitle = baseTitle;
            IsWithSubscription = isWithSubscription;

            Title = _baseTitle;

            if (IsWithSubscription)
                Title += SUBSCRIPTION_MARK;
        }

        public void AddSubscriptionMark()
        {
            Title += SUBSCRIPTION_MARK;
        }

        public void RemoveSubscriptionMark()
        {
            Title = _baseTitle;
        }
    }
}
