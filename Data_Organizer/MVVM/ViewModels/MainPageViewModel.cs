using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IClipboardService _clipboardService;

        [ObservableProperty]
        private FeatureModel _selectedFeature;
        [ObservableProperty]
        private LanguageModel _selectedLanguage;
        [ObservableProperty]
        private string _outputText;
        [ObservableProperty]
        private bool _isReadOnly;
        [ObservableProperty]
        private string _editButtonImageSource;

        public IFeatureService FeatureService { get; }
        public ICultureInfoService CultureInfoService { get; }
        public IAudioTranscriptorService AudioTranscriptorService { get; }

        public MainPageViewModel(
            IFeatureService featureService,
            ICultureInfoService cultureInfoService,
            INotificationService notificationService,
            IAudioTranscriptorService audioTranscriptorService,
            IClipboardService clipboardService)
        {
            FeatureService = featureService;
            CultureInfoService = cultureInfoService;
            AudioTranscriptorService = audioTranscriptorService;

            _notificationService = notificationService;
            _clipboardService = clipboardService;

            SetDefaultProperties();
        }

        private void SetDefaultProperties()
        {
            IsReadOnly = true;
            EditButtonImageSource = "disabled_edit_mode.png";
        }

        [RelayCommand]
        public async Task CopyOutputText()
        {
            if (string.IsNullOrWhiteSpace(OutputText))
            {
                await _notificationService.ShowToastAsync("Нема чого копіювати...");
                return;
            }

            await _clipboardService.Add(OutputText);

            await _notificationService.ShowToastAsync("Дані були успішно скопійовані!");
        }

        [RelayCommand]
        public async Task PasteText()
        {
            var action = await _notificationService.ShowActionSheetAsync("Як бажаєте вставити дані?", ["Вставити в кінець", "Замінити весь текст"]);

            switch (action)
            {
                case "Вставити в кінець":
                    OutputText += "\n";
                    OutputText += await _clipboardService.GetLastData();
                    await _notificationService.ShowToastAsync("Дані були успішно вставлені в кінець!");
                    break;
                case "Замінити весь текст":
                    OutputText = await _clipboardService.GetLastData();
                    await _notificationService.ShowToastAsync("Дані успішно замінили весь текст!");
                    break;
                default:
                    return;
            }
        }

        [RelayCommand]
        public async Task CleanEditor()
        {
            bool isConfirmed = await _notificationService.ShowConfirmationDialogAsync("Ви дійсно бажаєте очистити текстове поле?");

            if (!isConfirmed)
                return;

            OutputText = "";

            await _notificationService.ShowToastAsync("Текстове поле було очищено!");
        }

        [RelayCommand]
        public async Task SwitchEditMode()
        {
            bool wasEditModeEnabled = false;

            if (IsReadOnly)
                wasEditModeEnabled = await EnableEditMode();
            else
                await DisableEditMode();

            SwitchEditButtonImage(wasEditModeEnabled);
        }

        private void SwitchEditButtonImage(bool wasEditModeEnabled)
        {
            if (wasEditModeEnabled)
                EditButtonImageSource = "enabled_edit_mode.png";
            else
                EditButtonImageSource = "disabled_edit_mode.png";
        }

        public async Task<bool> EnableEditMode()
        {
            bool isConfirmed = await _notificationService.ShowConfirmationDialogAsync("Ви дійсно бажаєте увійти в режим редагування?");

            if (!isConfirmed)
                return false;

            IsReadOnly = false;

            await _notificationService.ShowToastAsync("Ви увійшли в режим редагування!");
            return true;
        }

        public async Task DisableEditMode()
        {
            IsReadOnly = true;

            await _notificationService.ShowToastAsync("Ви вийшли з режиму редагування!");
        }
    }
}
