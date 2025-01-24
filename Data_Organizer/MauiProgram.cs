using CommunityToolkit.Maui;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.Views;
using Data_Organizer.Services;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace Data_Organizer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IEnumDescriptionResolverService, EnumDescriptionResolverService>();
            builder.Services.AddSingleton<IPreferenceService, PreferenceService>();
            builder.Services.AddSingleton<IFeatureService, FeatureService>();
            builder.Services.AddSingleton<ICultureInfoService, CultureInfoService>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SavedNotesPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<MyTabbedPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
