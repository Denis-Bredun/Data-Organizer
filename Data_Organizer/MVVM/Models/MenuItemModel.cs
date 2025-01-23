namespace Data_Organizer.MVVM.Models
{
    public class MenuItemModel
    {
        public string Title { get; }
        public ImageSource? Icon { get; private set; }

        public MenuItemModel(string title)
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
