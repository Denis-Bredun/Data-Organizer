using CommunityToolkit.Mvvm.Input;

namespace Data_Organizer.MVVM.ViewModels.MainPageViewModel
{
    public partial class MainPageViewModel
    {
        [RelayCommand]
        public async Task ImportFile()
        {
            IsLoading = true;

            var fileInfoObj = await _fileService.ImportTextAsync();

            if (fileInfoObj != null)
            {
                _wasInfluenceOnOutputText = true;

                var importedText = fileInfoObj.Content;

                if (AreHeadersAdded)
                    AddHeaders(ref importedText, fileInfoObj.Name);

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

            var fileInfoObj = await _fileService.ImportAudiofileAsync();

            if (fileInfoObj != null)
            {
                _wasInfluenceOnOutputText = true;

                var transcribedText = fileInfoObj.Content;

                if (AreHeadersAdded)
                    AddHeaders(ref transcribedText, fileInfoObj.Name);

                SetOutputText(transcribedText);
            }

            IsLoading = false;
        }
    }
}