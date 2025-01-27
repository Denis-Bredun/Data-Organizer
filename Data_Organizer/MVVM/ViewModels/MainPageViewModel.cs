using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IAudioTranscriptorService _audioTranscriptorService;
        private readonly IClipboardService _clipboardService;

        [ObservableProperty]
        private FeatureModel _selectedFeature;
        [ObservableProperty]
        private LanguageModel _selectedLanguage;
        [ObservableProperty]
        private string _outputText;
        [ObservableProperty]
        private bool _isReadOnly;

        public IFeatureService FeatureService { get; }
        public ICultureInfoService CultureInfoService { get; }

        public MainPageViewModel(
            IFeatureService featureService,
            ICultureInfoService cultureInfoService,
            INotificationService notificationService,
            IAudioTranscriptorService audioTranscriptorService,
            IClipboardService clipboardService)
        {
            FeatureService = featureService;
            CultureInfoService = cultureInfoService;

            _notificationService = notificationService;
            _audioTranscriptorService = audioTranscriptorService;
            _clipboardService = clipboardService;

            IsReadOnly = true;
        }
    }
}
