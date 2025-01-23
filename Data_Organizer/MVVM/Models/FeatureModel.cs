namespace Data_Organizer.MVVM.Models
{
    public class FeatureModel
    {
        public string Title { get; }
        public ImageSource? Icon { get; private set; }
        public bool IsWithSubscription { get; set; }

        public FeatureModel(
            string title,
            bool isWithSubscription = false,
            string imagePath = "")
        {
            Title = title;
            IsWithSubscription = isWithSubscription;

            if (imagePath != "")
                Icon = ImageSource.FromFile(imagePath);
        }

        public void SetIcon(string imagePath)
        {
            Icon = ImageSource.FromFile(imagePath);
        }

        public void RemoveIcon()
        {
            Icon = null;
        }
    }
}
