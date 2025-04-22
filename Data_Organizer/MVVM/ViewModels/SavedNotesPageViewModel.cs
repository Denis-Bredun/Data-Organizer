using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Converters;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Data_Organizer.MVVM.ViewModels
{
    public partial class SavedNotesPageViewModel : ObservableObject
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IFirestoreDbService _firestoreDbService;
        private EventHandler _authStateChangedHandler;

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string statusMessage;
        [ObservableProperty] private bool isUserAuthorized;
        [ObservableProperty] private bool _doesnotRequireReloading;

        public bool AreNotesVisible => IsUserAuthorized && NoteHeaders.Count > 0;
        public ObservableCollection<NoteHeader> NoteHeaders { get; set; }

        public SavedNotesPageViewModel(
            IFirebaseAuthService firebaseAuthService,
            IFirestoreDbService firestoreDbService)
        {
            _firebaseAuthService = firebaseAuthService;
            _firestoreDbService = firestoreDbService;
            NoteHeaders = new();

            _authStateChangedHandler = OnAuthStateChanged;
            _firebaseAuthService.AuthStateChanged += _authStateChangedHandler;

            IsUserAuthorized = _firebaseAuthService.IsUserAuthorized();

            NoteHeaders.CollectionChanged += (_, _) => UpdateStatusMessage();

            UpdateStatusMessage();
        }

        private void OnAuthStateChanged(object sender, EventArgs e)
        {
            IsUserAuthorized = _firebaseAuthService.IsUserAuthorized();

            UpdateStatusMessage();
        }

        public void UpdateStatusMessage()
        {
            if (!IsUserAuthorized)
            {
                StatusMessage = "Ви не авторизовані, тому записів немає!";
            }
            else if (NoteHeaders.Count == 0)
            {
                StatusMessage = "У вас ще немає збережених записів!";
            }
            else
            {
                StatusMessage = string.Empty;
            }

            OnPropertyChanged(nameof(AreNotesVisible));
        }

        public async Task LoadNoteHeaders()
        {
            if (!DoesnotRequireReloading)
            {
                IsLoading = true;
                var noteHeaders = await _firestoreDbService.GetNoteHeadersByUidAsync();

                NoteHeaders.Clear();

                foreach (var noteHeader in noteHeaders)
                {
                    NoteHeaders.Add(noteHeader);
                }

                IsLoading = false;
                DoesnotRequireReloading = true;
            }
        }

        [RelayCommand]
        private async Task OpenNote(NoteHeader header)
        {
            IsLoading = true;
            var encoded = SerializeAndEncode(header);
            await Shell.Current.GoToAsync($"//EditNotePage?headerJson={encoded}");
        }

        private static string SerializeAndEncode(object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                Converters = { new DateTimeConverter() }
            };

            var json = JsonSerializer.Serialize(obj, options);
            return Uri.EscapeDataString(json);
        }


        [RelayCommand]
        public async Task DeleteNote(NoteHeader header)
        {
            IsLoading = true;
            var wasCompleted = await _firestoreDbService.RemoveNoteAsync(header);

            if (!wasCompleted)
            {
                IsLoading = false;
                return;
            }

            DoesnotRequireReloading = false;
            await LoadNoteHeaders();
            IsLoading = false;
        }
    }
}
