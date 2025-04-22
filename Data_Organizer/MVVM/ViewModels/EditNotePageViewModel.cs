using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Converters;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Text.Json;

namespace Data_Organizer.MVVM.ViewModels
{
    [QueryProperty(nameof(HeaderJson), "headerJson")]
    public partial class EditNotePageViewModel : ObservableObject
    {
        private readonly SavedNotesPageViewModel _savedNotesPageViewModel;
        private readonly IFirestoreDbService _firestoreDbService;

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private NoteHeader _header;
        [ObservableProperty] private string headerJson;
        [ObservableProperty] private NoteBody _body;

        public EditNotePageViewModel(
            IServiceProvider serviceProvider,
            IFirestoreDbService firestoreDbService)
        {
            Header = new NoteHeader();
            Body = new NoteBody();
            _savedNotesPageViewModel = serviceProvider.GetRequiredService<SavedNotesPageViewModel>();
            _firestoreDbService = firestoreDbService;
        }

        partial void OnHeaderJsonChanged(string value)
        {
            try
            {
                ProcessHeaderJson(value);
                LoadNoteBody();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization failed: {ex.Message}");
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ProcessHeaderJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "Переданий параметр \"header\" є null або порожній!");

            IsLoading = true;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                Converters = { new DateTimeConverter() }
            };

            var decoded = System.Web.HttpUtility.UrlDecode(value);
            Header = JsonSerializer.Deserialize<NoteHeader>(decoded, options)
                     ?? throw new JsonException("Не вдалося десеріалізувати NoteHeader.");
        }

        private void LoadNoteBody()
        {
            Body = new NoteBody
            {
                Content = $"Тут мав би бути вміст запису \"{Header.Title}\""
            };
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
            IsLoading = true;
            await Shell.Current.GoToAsync($"//SavedNotesPage");
        }
    }
}
