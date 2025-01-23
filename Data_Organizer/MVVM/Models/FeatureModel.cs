namespace Data_Organizer.MVVM.Models
{
    public class FeatureModel
    {
        public string Title { get; }
        public ImageSource? Icon { get; private set; }
        public bool IsWithSubscription { get; set; }

        public FeatureModel(string title)
        {
            Title = title;
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
