using Data_Organizer.MVVM.Views;

namespace Data_Organizer
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var tabbedPage = serviceProvider.GetRequiredService<MyTabbedPage>();
            MainPage = tabbedPage;
        }
    }
}
