using CommunityToolkit.Maui;
using Data_Organizer.APIRequestTools;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.Views;
using Data_Organizer.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("arialuni.ttf", "ArialUnicode");
                })
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial();

            var appSettings = GetAppSettings();
            var firebaseApiKey = GetFirebaseApiKey(appSettings);
            var firebaseAuthDomain = GetFirebaseAuthDomain(appSettings);
            var serverBaseURL = GetServerBaseURL(appSettings);

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = firebaseApiKey,
                AuthDomain = firebaseAuthDomain,
                Providers =
                        [
                            new EmailProvider()
                        ],
                UserRepository = new FileUserRepository("DataOrganizer")
            }));

            builder.Services.AddTransient<FirebaseAuthHttpMessageHandler>();

            builder.Services.AddRefitClient<IGetSummaryFromChatGPTQuery>().
                    ConfigureHttpClient(c => c.BaseAddress = new Uri(serverBaseURL)).
                    AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();

            builder.Services.AddRefitClient<IGetTranscriptionFromAudiofileQuery>().
                    ConfigureHttpClient(c => c.BaseAddress = new Uri(serverBaseURL)).
                    AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();

            builder.Services.AddTransient<IApplicationPreferencesService, ApplicationPreferencesService>();
            builder.Services.AddTransient<IEnumDescriptionResolverService, EnumDescriptionResolverService>();
            builder.Services.AddTransient<IPreferenceService, PreferenceService>();
            builder.Services.AddTransient<IFeatureService, FeatureService>();
            builder.Services.AddTransient<ICultureInfoService, CultureInfoService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
#if ANDROID
            builder.Services.AddTransient<ISpeechToTextService, Platforms.SpeechToTextService>();
#endif
            builder.Services.AddTransient<IAudioTranscriptorService, AudioTranscriptorService>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IFileServiceDecorator, FileServiceDecorator>();
            builder.Services.AddTransient<IClipboardService, ClipboardService>();
            builder.Services.AddTransient<IGoogleAuthenticationService, GoogleAuthenticationService>();
            builder.Services.AddTransient<IOpenAIAPIRequestService, OpenAIAPIRequestService>();
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

#if ANDROID
            Microsoft.Maui.Handlers.ScrollViewHandler.Mapper.ModifyMapping("HorizontalScrollBarVisibility", (handler, view, args) =>
            {
                handler.PlatformView.HorizontalScrollBarEnabled = true;
                handler.PlatformView.ScrollBarFadeDuration = 0;
            });

            Microsoft.Maui.Handlers.ScrollViewHandler.Mapper.ModifyMapping("VerticalScrollBarVisibility", (handler, view, args) =>
            {
                handler.PlatformView.VerticalScrollBarEnabled = true;
                handler.PlatformView.ScrollBarFadeDuration = 0;
            });
#endif

            return builder.Build();
        }

        private static IConfigurationRoot? GetAppSettings()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            using var stream = executingAssembly.GetManifestResourceStream("Data_Organizer.appsettings.json");
            var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

            return config;
        }

        private static string? GetFirebaseApiKey(IConfigurationRoot? appSettings)
        {
            var firebaseApiKeySetting = appSettings?.GetRequiredSection("FIREBASE_API_KEY");
            return firebaseApiKeySetting?.Value;
        }

        private static string? GetFirebaseAuthDomain(IConfigurationRoot? appSettings)
        {
            var firebaseAuthDomainSetting = appSettings?.GetRequiredSection("AUTH_DOMAIN");
            return firebaseAuthDomainSetting?.Value;
        }

        private static string? GetServerBaseURL(IConfigurationRoot? appSettings)
        {
            var serverBaseURLSetting = appSettings?.GetRequiredSection("SERVER_BASE_URL");
            return serverBaseURLSetting?.Value;
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
