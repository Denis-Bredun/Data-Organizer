using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel
    {
        [RelayCommand]
        public async Task PlayFeature()
        {
            if (SelectedFeature.Title.Contains("Транскрипція"))
                PlayTranscription();
            else if (SelectedFeature.Title.Contains("Конспект"))
                await PlayAISummary();
        }

        private async Task PlayAISummary()
        {
            if (string.IsNullOrWhiteSpace(OutputText))
            {
                await _notificationService.ShowToastAsync("Тези не можуть бути зроблені з пустоти!");
                return;
            }

            IsLoading = true;

            var responseResult = await _openAIAPIRequestService.GetSummaryAsync(OutputText, SelectedLanguage);

            if (responseResult != null)
            {
                _wasInfluenceOnOutputText = true;
                SetOutputText(responseResult.Result);
            }

            IsLoading = false;
        }

        private void PlayTranscription()
        {
            SetTranscriptionUpdatedHandlerIfNecessary();

            if (!AudioTranscriptorService.IsListening)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(SelectedLanguage.CultureCode);

                if (!IsTextAddedAtTheEnd)
                {
                    OutputText = "";
                    SetTranscriptionFromOutputText();
                }
                else if (_wasInfluenceOnOutputText)
                    SetTranscriptionFromOutputText(_lineToDivideOutput);

                AudioTranscriptorService.StartListeningAsync(cultureInfo);
            }
            else
            {
                _wasInfluenceOnOutputText = false;
                AudioTranscriptorService.StopListening();
            }
        }

        private void SetTranscriptionUpdatedHandlerIfNecessary()
        {
            if (_transcriptionUpdatedHandler == null)
            {
                _transcriptionUpdatedHandler = text => OutputText = text;
                SetTranscriptionFromOutputText();
                AudioTranscriptorService.OnTranscriptionUpdated += _transcriptionUpdatedHandler;
            }
        }

        private void SetTranscriptionFromOutputText(string textToAdd = "")
        {
            OutputText += textToAdd;
            AudioTranscriptorService.SetTranscription(OutputText);
        }

        public void SwitchPlayButtonImage(bool isListening)
        {
            if (isListening)
                PlayButtonImageSource = "pause_record.svg";
            else
                PlayButtonImageSource = "start_record.svg";
        }

        private void UnsubscribeFromTranscriptionUpdates()
        {
            if (_transcriptionUpdatedHandler != null)
            {
                AudioTranscriptorService.OnTranscriptionUpdated -= _transcriptionUpdatedHandler;
                _transcriptionUpdatedHandler = null;
            }
        }

        [RelayCommand]
        public async Task ImportAudioFile()
        {
            IsLoading = true;

            var languageOptions = CultureInfoService.Languages.Select(l => l.DisplayName).ToArray();
            var answer = await _notificationService.ShowActionSheetAsync("Яка мова в аудіофайлі?", languageOptions);

            if (answer != "Нічого" && !string.IsNullOrEmpty(answer))
            {
                var selectedLanguage = CultureInfoService.Languages.FirstOrDefault(l => l.DisplayName == answer);
                string transcribedText = await _fileService.ImportAudiofileAsync(selectedLanguage);

                if (transcribedText != null)
                {
                    _wasInfluenceOnOutputText = true;
                    SetOutputText(transcribedText);
                }
            }

            IsLoading = false;
        }
    }
} 