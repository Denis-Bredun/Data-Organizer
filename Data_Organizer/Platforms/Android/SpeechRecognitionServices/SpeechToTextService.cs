using Android.Content;
using Android.Speech;
using Data_Organizer.Interfaces;
using System.Globalization;

namespace Data_Organizer.Platforms
{
    public class SpeechToTextService : ISpeechToTextService
    {
        private SpeechRecognitionListener? _listener;
        private SpeechRecognizer? _speechRecognizer;

        public async Task<bool> RequestPermissionsAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            var isAvailable = SpeechRecognizer.IsRecognitionAvailable(Android.App.Application.Context);
            return status == PermissionStatus.Granted && isAvailable;
        }

        public async Task<string> ListenAsync(CultureInfo culture,
            IProgress<string> recognitionResult,
            CancellationToken cancellationToken)
        {
            var taskResult = new TaskCompletionSource<string>();

            _listener = new SpeechRecognitionListener
            {
                Error = ex => taskResult.TrySetException(new Exception($"Збій мовного механізму - {ex}!")),
                PartialResults = sentence =>
                {
                    recognitionResult?.Report(sentence);
                },
                Results = sentence => taskResult.TrySetResult(sentence)
            };

            _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Android.App.Application.Context);

            if (_speechRecognizer is null)
                throw new NullReferenceException("Розпізнавач мовлення недоступний!");

            _speechRecognizer.SetRecognitionListener(_listener);
            _speechRecognizer.StartListening(CreateSpeechIntent(culture));

            await using (cancellationToken.Register(() =>
            {
                StopRecording();
                taskResult.TrySetCanceled();
            }))
            {
                return await taskResult.Task;
            }
        }

        private Intent CreateSpeechIntent(CultureInfo culture)
        {
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

            intent.PutExtra(RecognizerIntent.ExtraLanguage, culture.Name);
            intent.PutExtra(RecognizerIntent.ExtraLanguagePreference, culture.Name);

            var javaLocale = Java.Util.Locale.ForLanguageTag(culture.Name);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, Android.App.Application.Context.PackageName);

            intent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, int.MaxValue);
            intent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, int.MaxValue);
            intent.PutExtra(RecognizerIntent.ExtraPartialResults, true);

            return intent;
        }

        private void StopRecording()
        {
            _speechRecognizer?.StopListening();
            _speechRecognizer?.Destroy();
        }
    }
}
