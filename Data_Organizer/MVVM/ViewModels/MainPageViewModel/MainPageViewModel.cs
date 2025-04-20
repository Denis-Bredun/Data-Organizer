using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel : ObservableObject, IDisposable
    {
        private readonly INotificationService _notificationService;
        private readonly IOpenAIAPIRequestService _openAIAPIRequestService;
        private readonly IClipboardService _clipboardService;
        private readonly IFileServiceDecorator _fileService;
        private readonly IFirestoreDbService _firestoreDbService;

        private Action<string>? _transcriptionUpdatedHandler;
        private bool _wasInfluenceOnOutputText;
        private string _lineToDivideOutput;

        [ObservableProperty] private FeatureModel _selectedFeature;
        [ObservableProperty] private LanguageModel _selectedLanguage;
        [ObservableProperty] private string _outputText;
        [ObservableProperty] private bool _isReadOnly;
        [ObservableProperty] private string _editButtonImageSource;
        [ObservableProperty] private string _playButtonImageSource;
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private bool _areSettingsOpen;
        [ObservableProperty] private bool _isTextAddedAtTheEnd;
        [ObservableProperty] private bool _areHeadersAdded;
        [ObservableProperty] private bool _hasVisitedMainPage;

        public ICultureInfoService CultureInfoService { get; }
        public IAudioTranscriptorService AudioTranscriptorService { get; }

        public ObservableCollection<FeatureModel> Features { get; }

        public MainPageViewModel(
            ICultureInfoService cultureInfoService,
            INotificationService notificationService,
            IAudioTranscriptorService audioTranscriptorService,
            IOpenAIAPIRequestService openAIAPIRequestService,
            IClipboardService clipboardService,
            IFileServiceDecorator fileService,
            IFirestoreDbService firestoreDbService)
        {
            CultureInfoService = cultureInfoService;
            AudioTranscriptorService = audioTranscriptorService;

            _notificationService = notificationService;
            _openAIAPIRequestService = openAIAPIRequestService;
            _clipboardService = clipboardService;
            _fileService = fileService;
            _firestoreDbService = firestoreDbService;
            _lineToDivideOutput = "\n-----------------------\n";

            Features = new ObservableCollection<FeatureModel>()
            {
                new FeatureModel("Транскрипція", Enums.Features.Transcription),
                new FeatureModel("Конспект", Enums.Features.Summary)
            };

            SetDefaultProperties();
        }

        private void SetDefaultProperties()
        {
            IsReadOnly = true;
            AreHeadersAdded = true;
            EditButtonImageSource = "disabled_edit_mode.svg";
            PlayButtonImageSource = "start_record.svg";
        }

        private void SetOutputText(string newText)
        {
            UnsubscribeFromTranscriptionUpdates();

            if (IsTextAddedAtTheEnd && !string.IsNullOrEmpty(OutputText))
                OutputText += _lineToDivideOutput + newText;
            else
                OutputText = newText;
        }

        public void Dispose()
        {
            AudioTranscriptorService.StopListening();
            UnsubscribeFromTranscriptionUpdates();
        }
    }
}