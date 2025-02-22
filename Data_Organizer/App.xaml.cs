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

            VersionTracking.Track();

            var appShell = serviceProvider.GetRequiredService<AppShell>();
            MainPage = appShell;

            _applicationPreferencesService = serviceProvider.GetRequiredService<IApplicationPreferencesService>();
        }

        protected override void OnSleep()
        {
            _applicationPreferencesService.SavePreferences();

            base.OnSleep();
        }

        protected override async void OnStart()
        {
            _applicationPreferencesService.LoadPreferences();

            if (!VersionTracking.IsFirstLaunchEver)
                await Shell.Current.GoToAsync("//TabBar");

            base.OnStart();
        }
    }
}
