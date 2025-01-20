using Data_Organizer.MVVM.Views;

namespace Data_Organizer
{
    public partial class App : Application
    {
        public App(MyTabbedPage tabbedPage)
        {
            InitializeComponent();

            MainPage = tabbedPage;
        }
    }
}
