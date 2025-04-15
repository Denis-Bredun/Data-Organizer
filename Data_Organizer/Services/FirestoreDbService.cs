using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.Queries;
using Data_Organizer_Server.Models;

namespace Data_Organizer.Services
{
    public class FirestoreDbService : IFirestoreDbService
    {
        private readonly SavedNotesPageViewModel _savedNotesPageViewModel;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IFirestoreDbQueries _firestoreDbQueries;
        private readonly INotificationService _notificationService;

        public FirestoreDbService(
            IServiceProvider serviceProvider,
            IFirebaseAuthService firebaseAuthService,
            IFirestoreDbQueries firestoreDbQueries,
            INotificationService notificationService)
        {
            _savedNotesPageViewModel = serviceProvider.GetRequiredService<SavedNotesPageViewModel>();
            _firebaseAuthService = firebaseAuthService;
            _firestoreDbQueries = firestoreDbQueries;
            _notificationService = notificationService;
        }

        public async Task CreateUser(bool isMetadataStored)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            UserRequestDTO userCreationRequest = new UserRequestDTO();

            userCreationRequest.User = new User()
            {
                Uid = _firebaseAuthService.GetUid(),
                IsMetadataStored = isMetadataStored
            };

            if (isMetadataStored)
            {

            }
        }
    }
}
