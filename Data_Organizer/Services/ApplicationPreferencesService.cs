using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.Services
{
    public class ApplicationPreferencesService : IApplicationPreferencesService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MainPageViewModel _mainPageViewModel;

        public ApplicationPreferencesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();
        }

        public void LoadPreferences()
        {
            Console.WriteLine();
        }

        public void SavePreferences()
        {
            Console.WriteLine();
        }
    }
}
