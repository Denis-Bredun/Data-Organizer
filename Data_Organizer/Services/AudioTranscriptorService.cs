using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using System.Globalization;

namespace Data_Organizer.Services
{
    public partial class AudioTranscriptorService : ObservableObject, IAudioTranscriptorService
    {
        public event Action<string> OnTranscriptionUpdated;

        private readonly ISpeechToTextService _speechToTextService;
        private readonly INotificationService _notificationService;
        private readonly IServiceProvider _serviceProvider;
        private MainPageViewModel _mainPageViewModel;
        private CancellationTokenSource _tokenSource;
        private string _transcription;

        [ObservableProperty]
        private bool _isListening;

        public AudioTranscriptorService(
            ISpeechToTextService speechToTextService,
            INotificationService notificationService,
            IServiceProvider serviceProvider)
        {
            _speechToTextService = speechToTextService;
            _notificationService = notificationService;
            _serviceProvider = serviceProvider;

            IsListening = false;
        }

        public async Task StartListeningAsync(CultureInfo culture)
        {
            if (!await CheckInternetConnectionAsync())
                return;

            if (!await CheckPermissionsAsync())
                return;

            SetMainPageViewModelIfNecessary();

            ResetCancellationTokenSource();

            try
            {
                IsListening = true;
                SwitchPlayButtonImage();
                await LaunchSpeechToTextServiceAsync(culture);
            }
            catch (Exception ex)
            {
                IsListening = false;
                SwitchPlayButtonImage();
                await ShowNotificationWhenStoppedListeningAsync(
                    _tokenSource.IsCancellationRequested,
                    ex.Message);
            }
        }

        private void SetMainPageViewModelIfNecessary()
        {
            if (_mainPageViewModel == null)
                _mainPageViewModel = _serviceProvider.GetRequiredService<MainPageViewModel>();
        }

        private void SwitchPlayButtonImage()
        {
            _mainPageViewModel.SwitchPlayButtonImage(IsListening);
        }

        public void StopListening()
        {
            _tokenSource?.Cancel();
        }

        private bool IsConnectedToInternet() => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        private async Task<bool> CheckInternetConnectionAsync()
        {
            return await CheckConditionAsync(
                async () => IsConnectedToInternet(),
                "Немає Інтернет-підключення"
            );
        }

        private async Task<bool> CheckPermissionsAsync()
        {
            return await CheckConditionAsync(
                _speechToTextService.RequestPermissionsAsync,
                "Немає доступу до мікрофону"
            );
        }

        private async Task<bool> CheckConditionAsync(Func<Task<bool>> condition, string errorMessage)
        {
            var result = await condition();

            if (!result)
                await _notificationService.ShowToastAsync(errorMessage);

            return result;
        }

        private void ResetCancellationTokenSource() => _tokenSource = new CancellationTokenSource();

        private async Task LaunchSpeechToTextServiceAsync(CultureInfo culture)
        {
            string baseText = "";
            bool addNewLine = false;
            var lastUpdateTime = DateTime.UtcNow;

            async Task MonitorSilence()
            {
                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    if ((DateTime.UtcNow - lastUpdateTime).TotalSeconds >= 10)
                    {
                        _tokenSource.Cancel();
                    }
                }
            }

            _ = MonitorSilence();

            _transcription = await _speechToTextService.ListenAsync(
                culture,
                new Progress<string>(currentText =>
                {
                    lastUpdateTime = DateTime.UtcNow;
                    FormatRecognizedSpeech(ref baseText, ref currentText, ref addNewLine);
                    OnTranscriptionUpdated(_transcription);
                }), _tokenSource.Token);
        }

        private void FormatRecognizedSpeech(ref string baseText, ref string currentText, ref bool addNewLine)
        {
            if (currentText == "")
            {
                baseText = _transcription;
                addNewLine = true;
            }

            if (!addNewLine)
            {
                _transcription = $"{baseText}{currentText}";
                return;
            }

            if (DoesntTransferToNewLine(baseText))
                baseText += ". \n";

            _transcription = $"{baseText}{currentText}";
            addNewLine = false;
        }

        private bool DoesntTransferToNewLine(string baseText) => !string.IsNullOrEmpty(baseText) && !baseText.EndsWith("\n");

        private async Task ShowNotificationWhenStoppedListeningAsync(
            bool isCancellationRequested,
            string exceptionMessage)
        {
            string notification = isCancellationRequested ? "Запис був зупинений" : exceptionMessage;

            await _notificationService.ShowToastAsync(notification);
        }

        public void SetTranscription(string outputText)
        {
            _transcription = outputText;
        }
    }
}
