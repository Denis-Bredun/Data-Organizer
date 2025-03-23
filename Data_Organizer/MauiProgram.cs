﻿using CommunityToolkit.Maui;
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
            
            ConfigureAppEssentials(builder);
            
            var config = LoadConfiguration();
            
            RegisterFirebase(builder.Services, config);
            
            builder.Services
                .AddApiClients(config["SERVER_BASE_URL"])
                .AddAppServices()
                .AddViewModels();
                
            ConfigurePlatformSpecifics();
            
            return builder.Build();
        }

        private static void ConfigureAppEssentials(MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => fonts.AddFont("arialuni.ttf", "ArialUnicode"))
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial();
                
#if DEBUG
            builder.Logging.AddDebug();
#endif
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            using var stream = executingAssembly.GetManifestResourceStream("Data_Organizer.appsettings.json");
            return new ConfigurationBuilder().AddJsonStream(stream).Build();
        }

        private static void RegisterFirebase(IServiceCollection services, IConfigurationRoot config)
        {
            var firebaseApiKey = config["FIREBASE_API_KEY"];
            var firebaseAuthDomain = config["AUTH_DOMAIN"];
            
            services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = firebaseApiKey,
                AuthDomain = firebaseAuthDomain,
                Providers = [new EmailProvider()],
                UserRepository = new FileUserRepository("DataOrganizer")
            }));
        }

        private static void ConfigurePlatformSpecifics()
        {
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
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services, string baseUrl)
        {
            services.AddTransient<FirebaseAuthHttpMessageHandler>();
            
            services.AddRefitClient<IGetSummaryFromChatGPTQuery>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                    .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();
                    
            services.AddRefitClient<IGetTranscriptionFromAudiofileQuery>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                    .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();
                    
            return services;
        }
        
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicationPreferencesService, ApplicationPreferencesService>();
            services.AddTransient<IEnumDescriptionResolverService, EnumDescriptionResolverService>();
            services.AddTransient<IPreferenceService, PreferenceService>();
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddTransient<ICultureInfoService, CultureInfoService>();
            services.AddTransient<INotificationService, NotificationService>();
#if ANDROID
            services.AddTransient<ISpeechToTextService, Platforms.SpeechToTextService>();
#endif
            services.AddTransient<IAudioTranscriptorService, AudioTranscriptorService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IFileServiceDecorator, FileServiceDecorator>();
            services.AddTransient<IClipboardService, ClipboardService>();
            services.AddTransient<IGoogleAuthenticationService, GoogleAuthenticationService>();
            services.AddTransient<IOpenAIAPIRequestService, OpenAIAPIRequestService>();
            
            services.AddTransient<AppShell>();
            services.AddTransient<SavedNotesPage>();
            services.AddTransient<SettingsPage>();
            
            return services;
        }
        
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddViewModel<WelcomeViewModel, WelcomePage>();
            services.AddViewModel<SignUpViewModel, SignUpPage>();
            services.AddViewModel<SignInViewModel, SignInPage>();
            services.AddViewModel<MainPageViewModel, MainPage>();
            
            return services;
        }
        
        public static void AddViewModel<TViewModel, TView>(this IServiceCollection services)
            where TView : ContentPage, new()
            where TViewModel : class
        {
            services.AddSingleton<TViewModel>();
            services.AddTransient<TView>(s => new TView { BindingContext = s.GetRequiredService<TViewModel>() });
        }
    }
}
