using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Views;

namespace Data_Organizer
{
    public partial class App : Application
    {
        private readonly IApplicationPreferencesService _applicationPreferencesService;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var tabbedPage = serviceProvider.GetRequiredService<MyTabbedPage>();
            MainPage = tabbedPage;

            _applicationPreferencesService = serviceProvider.GetRequiredService<IApplicationPreferencesService>();
        }

        protected override void OnSleep()
        {
            _applicationPreferencesService.SavePreferences();

            base.OnSleep();
        }

        protected override void OnStart()
        {
            _applicationPreferencesService.LoadPreferences();

            base.OnStart();
        }
    }
}
