using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Converters;
using Data_Organizer.MVVM.Models;
using System.Text.Json;

namespace Data_Organizer.MVVM.ViewModels
{
    [QueryProperty(nameof(HeaderJson), "headerJson")]
    public partial class EditNotePageViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private NoteHeader _header;
        [ObservableProperty] private string headerJson;
        [ObservableProperty] private NoteBody _body;

        public EditNotePageViewModel()
        {
            Header = new NoteHeader();
            Body = new NoteBody();
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
    }
}
