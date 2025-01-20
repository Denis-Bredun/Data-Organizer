namespace Data_Organizer.MVVM.Models
{
    public class MenuItemModel
    {
        public string Title { get; set; }
        public ImageSource? Icon { get; private set; }

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
