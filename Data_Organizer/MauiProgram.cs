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
                .UseUraniumUIMaterial();

            builder.Services.AddTransient<IApplicationPreferencesService, ApplicationPreferencesService>();
            builder.Services.AddTransient<IEnumDescriptionResolverService, EnumDescriptionResolverService>();
            builder.Services.AddTransient<IPreferenceService, PreferenceService>();
            builder.Services.AddTransient<IFeatureService, FeatureService>();
            builder.Services.AddTransient<ICultureInfoService, CultureInfoService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<ISpeechToTextService, Data_Organizer.Platforms.SpeechToTextService>();
            builder.Services.AddTransient<IAudioTranscriptorService, AudioTranscriptorService>();
            builder.Services.AddTransient<IClipboardService, ClipboardService>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddViewModel<WelcomeViewModel, WelcomePage>();
            builder.Services.AddViewModel<SignUpViewModel, SignUpPage>();
            builder.Services.AddViewModel<SignInViewModel, SignInPage>();
            builder.Services.AddViewModel<MainPageViewModel, MainPage>();
            builder.Services.AddViewModel<MainPageViewModel, MainPage>();
            builder.Services.AddTransient<SavedNotesPage>();
            builder.Services.AddTransient<SettingsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void AddViewModel<TViewModel, TView>(this IServiceCollection services)
            where TView : ContentPage, new()
            where TViewModel : class
        {
            services.AddSingleton<TViewModel>();
            services.AddTransient<TView>(s => new TView() { BindingContext = s.GetRequiredService<TViewModel>() });
        }
    }
}
