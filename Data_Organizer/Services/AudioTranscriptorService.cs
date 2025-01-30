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
        private bool isListening;

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

        public async Task StartListening(CultureInfo culture)
        {
            if (!await CheckInternetConnection())
                return;

            if (!await CheckPermissions())
                return;

            SetMainPageViewModelIfNecessary();

            ResetCancellationTokenSource();

            try
            {
                IsListening = true;
                SwitchPlayButtonImage();
                await LaunchSpeechToTextService(culture);
            }
            catch (Exception ex)
            {
                IsListening = false;
                SwitchPlayButtonImage();
                await ShowNotificationWhenStoppedListening(
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

        private async Task<bool> CheckInternetConnection()
        {
            return await CheckConditionAsync(
                async () => IsConnectedToInternet(),
                "Немає Інтернет-підключення"
            );
        }

        private async Task<bool> CheckPermissions()
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

        private async Task LaunchSpeechToTextService(CultureInfo culture)
        {
            string baseText = "";
            bool addNewLine = false;

            _transcription = await _speechToTextService.ListenAsync(
                culture,
                new Progress<string>(currentText =>
                {
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

        private async Task ShowNotificationWhenStoppedListening(
            bool isCancellationRequested,
            string exceptionMessage)
        {
            string notification = isCancellationRequested ? "Запис був зупинений" : exceptionMessage;

            await _notificationService.ShowToastAsync(notification);
        }
    }
}
