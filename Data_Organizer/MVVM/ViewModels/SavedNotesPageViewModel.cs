using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using System.Collections.ObjectModel;

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
        [ObservableProperty] private bool _wasLoadedOnce;

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
            if (!WasLoadedOnce)
            {
                IsLoading = true;
                var noteHeaders = await _firestoreDbService.GetNoteHeadersByUidAsync();

                foreach (var noteHeader in noteHeaders)
                {
                    NoteHeaders.Add(noteHeader);
                }

                IsLoading = false;
                WasLoadedOnce = true;
            }
        }

        [RelayCommand]
        private void OpenNote()
        {
        }

        [RelayCommand]
        private void DeleteNote()
        {
        }
    }
}
