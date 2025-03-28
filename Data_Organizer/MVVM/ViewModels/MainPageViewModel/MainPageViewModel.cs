﻿using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel : ObservableObject, IDisposable
    {
        private readonly INotificationService _notificationService;
        private readonly IOpenAIAPIRequestService _openAIAPIRequestService;
        private readonly IClipboardService _clipboardService;
        private readonly IFileServiceDecorator _fileService;

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
        [ObservableProperty] private bool _isHelpOpen;

        public IFeatureService FeatureService { get; }
        public ICultureInfoService CultureInfoService { get; }
        public IAudioTranscriptorService AudioTranscriptorService { get; }

        public MainPageViewModel(
            IFeatureService featureService,
            ICultureInfoService cultureInfoService,
            INotificationService notificationService,
            IAudioTranscriptorService audioTranscriptorService,
            IOpenAIAPIRequestService openAIAPIRequestService,
            IClipboardService clipboardService,
            IFileServiceDecorator fileService)
        {
            FeatureService = featureService;
            CultureInfoService = cultureInfoService;
            AudioTranscriptorService = audioTranscriptorService;

            _notificationService = notificationService;
            _openAIAPIRequestService = openAIAPIRequestService;
            _clipboardService = clipboardService;
            _fileService = fileService;
            _lineToDivideOutput = "\n-----------------------\n";

            SetDefaultProperties();
        }

        private void SetDefaultProperties()
        {
            IsReadOnly = true;
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