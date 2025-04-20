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

        public ObservableCollection<NoteHeader> NoteHeaders { get; set; }

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string statusMessage;
        [ObservableProperty] private bool isUserAuthorized;

        public bool AreNotesVisible => IsUserAuthorized && NoteHeaders.Count > 0;

        private EventHandler _authStateChangedHandler;

        public SavedNotesPageViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
            NoteHeaders = new();

            _authStateChangedHandler = OnAuthStateChanged;
            _firebaseAuthService.AuthStateChanged += _authStateChangedHandler;

            IsUserAuthorized = _firebaseAuthService.IsUserAuthorized();

            NoteHeaders.CollectionChanged += (_, _) => UpdateStatusMessage();

            for (int i = 1; i <= 10; i++)
            {
                NoteHeaders.Add(new NoteHeader
                {
                    Title = $"Заголовок {i} - це текст, що складається",
                    PreviewText = $"Це довгий опис запису №{i}, що складається із 70 символів для тестування даного фрагмента тексту в додатку.",
                    CreationTime = DateTime.Now.AddMinutes(-i * 10)
                });
            }

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
