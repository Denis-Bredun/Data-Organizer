using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.ComponentModel;
using System.Text.Json;

namespace Data_Organizer.MVVM.ViewModels
{
    [QueryProperty(nameof(HeaderJson), "headerJson")]
    public partial class EditNotePageViewModel : ObservableObject
    {
        private readonly SavedNotesPageViewModel _savedNotesPageViewModel;
        private readonly IFirestoreDbService _firestoreDbService;
        private readonly INotificationService _notificationService;
        private bool _hasChanges;

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private NoteHeader _header;
        [ObservableProperty] private string _headerJson;
        [ObservableProperty] private NoteBody _body;

        public EditNotePageViewModel(
            IServiceProvider serviceProvider,
            IFirestoreDbService firestoreDbService,
            INotificationService notificationService)
        {
            Header = new NoteHeader();
            Body = new NoteBody();

            Header.PropertyChanged += OnNotePropertyChanged;
            Body.PropertyChanged += OnNotePropertyChanged;

            _savedNotesPageViewModel = serviceProvider.GetRequiredService<SavedNotesPageViewModel>();
            _firestoreDbService = firestoreDbService;
            _notificationService = notificationService;
        }

        private void OnNotePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteHeader.Title) || e.PropertyName == nameof(NoteBody.Content))
            {
                _hasChanges = true;
            }
        }

        partial void OnHeaderJsonChanged(string value)
        {
            if (value == "Exit")
                return;

            Task.Run(async () =>
            {
                try
                {
                    ProcessHeaderJson(value);
                    await LoadNoteBody();
                    await _notificationService.ShowToastAsync("Запис було успішно завантажено!");
                }
                catch (Exception ex)
                {
                    await _notificationService.ShowToastAsync($"Помилка при обробці header: {ex.Message}");
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        private void ProcessHeaderJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "Переданий параметр \"header\" є null або порожній!");

            IsLoading = true;

            if (Header != null)
                Header.PropertyChanged -= OnNotePropertyChanged;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                Converters = { new Converters.DateTimeConverter() }
            };

            var decoded = System.Web.HttpUtility.UrlDecode(value);
            Header = JsonSerializer.Deserialize<NoteHeader>(decoded, options)
                     ?? throw new JsonException("Не вдалося десеріалізувати NoteHeader.");

            Header.PropertyChanged += OnNotePropertyChanged;
            IsLoading = false;
        }

        private async Task LoadNoteBody()
        {
            IsLoading = true;

            if (Body != null)
                Body.PropertyChanged -= OnNotePropertyChanged;

            Body = await _firestoreDbService.GetNoteBodyByHeaderAsync(Header);

            if (Body != null)
                Body.PropertyChanged += OnNotePropertyChanged;

            IsLoading = false;
        }

        [RelayCommand]
        public async Task DeleteNote()
        {
            IsLoading = true;
            var wasCompleted = await _firestoreDbService.RemoveNoteAsync(Header);

            if (!wasCompleted)
            {
                IsLoading = false;
                return;
            }

            _savedNotesPageViewModel.DoesnotRequireReloading = false;
            await Shell.Current.GoToAsync($"//SavedNotesPage");
        }

        [RelayCommand]
        public async Task Back()
        {
            if (_hasChanges)
            {
                var wasConfirmed = await _notificationService.ShowConfirmationDialogAsync(
                    "Залишились незбережені зміни. Зберегти?");

                if (wasConfirmed)
                {
                    var succeded = await UpdateNote();

                    if (!succeded)
                        return;
                }
            }

            IsLoading = true;
            HeaderJson = "Exit";
            await Shell.Current.GoToAsync("//SavedNotesPage");
        }

        [RelayCommand]
        public async Task<bool> UpdateNote()
        {
            IsLoading = true;
            var succeded = await _firestoreDbService.UpdateNoteAsync(Header, Body);

            if (succeded)
            {
                _savedNotesPageViewModel.DoesnotRequireReloading = false;
                _hasChanges = false;
            }

            IsLoading = false;

            return succeded;
        }
    }
}
