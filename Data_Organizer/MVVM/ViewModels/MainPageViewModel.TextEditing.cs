using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class MainPageViewModel
    {
        [RelayCommand]
        public async Task CopyOutputText()
        {
            if (string.IsNullOrWhiteSpace(OutputText))
            {
                await _notificationService.ShowToastAsync("Нема чого копіювати...");
                return;
            }

            await _clipboardService.AddAsync(OutputText);

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
                    OutputText += await _clipboardService.GetLastDataAsync();
                    await _notificationService.ShowToastAsync("Дані були успішно вставлені в кінець!");
                    break;
                case "Замінити весь текст":
                    OutputText = await _clipboardService.GetLastDataAsync();
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
            _wasInfluenceOnOutputText = false;
            UnsubscribeFromTranscriptionUpdates();

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
                EditButtonImageSource = "enabled_edit_mode.svg";
            else
                EditButtonImageSource = "disabled_edit_mode.svg";
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