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

            await _fileService.ExportTextAsync(OutputText);

            IsLoading = false;
        }

        [RelayCommand]
        public async Task ImportAudioFile()
        {
            IsLoading = true;

            string transcribedText = await _fileService.ImportAudiofileAsync();

            if (transcribedText != null)
            {
                _wasInfluenceOnOutputText = true;
                SetOutputText(transcribedText);
            }

            IsLoading = false;
        }
    }
}