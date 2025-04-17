using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
using Data_Organizer.MVVM.Views;

namespace Data_Organizer
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationPreferencesService _applicationPreferencesService;
        private readonly IFirestoreDbService _firestoreDbService;
        private readonly IFirebaseAuthService _firebaseAuthService;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            VersionTracking.Track();

            var appShell = serviceProvider.GetRequiredService<AppShell>();
            MainPage = appShell;

            _serviceProvider = serviceProvider;
            _applicationPreferencesService = serviceProvider.GetRequiredService<IApplicationPreferencesService>();
            _firestoreDbService = serviceProvider.GetRequiredService<IFirestoreDbService>();
            _firebaseAuthService = serviceProvider.GetRequiredService<IFirebaseAuthService>();
        }

        protected override void OnSleep()
        {
            _applicationPreferencesService.SavePreferences();

            base.OnSleep();
        }

        protected override async void OnStart()
        {
            _applicationPreferencesService.LoadPreferences();

            var mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();

            if (mainPageViewModel.HasVisitedMainPage)
                await Shell.Current.GoToAsync("//TabBar");

            base.OnStart();
        }
    }
}
