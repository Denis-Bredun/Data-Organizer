using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel
    {
        [RelayCommand]
        public async Task ImportFile()
        {
            IsLoading = true;

            string importedText = await _fileService.ImportTextAsync();

            if (importedText != null)
            {
                _wasInfluenceOnOutputText = true;
                SetOutputText(importedText);
            }

            IsLoading = false;
        }

        [RelayCommand]
        public async Task ExportFile()
        {
            IsLoading = true;

            const string
                pdf = "PDF",
                docx = "DOCX",
                txt = "TXT";

            var answer = await _notificationService.ShowActionSheetAsync("В якому розширенні бажаєте експортувати?", pdf, docx, txt);

            switch (answer)
            {
                case pdf:
                    await _fileService.ExportTextAsync(OutputText, AppEnums.TextFileFormat.PDF);
                    break;
                case docx:
                    await _fileService.ExportTextAsync(OutputText, AppEnums.TextFileFormat.DOCX);
                    break;
                case txt:
                    await _fileService.ExportTextAsync(OutputText, AppEnums.TextFileFormat.TXT);
                    break;
            }

            IsLoading = false;
        }
    }
}