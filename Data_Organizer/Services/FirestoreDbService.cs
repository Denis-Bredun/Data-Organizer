using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.Models;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.Queries;

namespace Data_Organizer.Services
{
    public class FirestoreDbService : IFirestoreDbService
    {
        private readonly SavedNotesPageViewModel _savedNotesPageViewModel;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IFirestoreDbQueries _firestoreDbQueries;
        private readonly INotificationService _notificationService;
        private readonly IDeviceServiceDecorator _deviceServiceDecorator;
        private readonly IMappingService _mappingService;

        public FirestoreDbService(
            IServiceProvider serviceProvider,
            IFirebaseAuthService firebaseAuthService,
            IFirestoreDbQueries firestoreDbQueries,
            INotificationService notificationService,
            IDeviceServiceDecorator deviceServiceDecorator,
            IMappingService mappingService)
        {
            _savedNotesPageViewModel = serviceProvider.GetRequiredService<SavedNotesPageViewModel>();
            _firebaseAuthService = firebaseAuthService;
            _firestoreDbQueries = firestoreDbQueries;
            _notificationService = notificationService;
            _deviceServiceDecorator = deviceServiceDecorator;
            _mappingService = mappingService;
        }

        public async Task CreateUserAsync(bool isMetadataStored, Data_Organizer.Models.Location location)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            UserRequestDTO userRequestDTO = new();

            var user = new User()
            {
                Uid = _firebaseAuthService.GetUid(),
                IsMetadataStored = isMetadataStored
            };

            userRequestDTO.UserDTO = _mappingService.MapUser(user);

            if (isMetadataStored)
            {
                var deviceInfo = _deviceServiceDecorator.GetDeviceInfo();
                var dateTimeNow = DateTime.Now;

                var metadata = new UsersMetadata()
                {
                    CreationDate = dateTimeNow,
                    CreationLocation = location
                };

                userRequestDTO.UsersMetadataDTO = _mappingService.MapMetadata(metadata);

                userRequestDTO.CreationDevice = deviceInfo;
            }

            UserRequestDTO? response = new UserRequestDTO();

            try
            {
                response = await _firestoreDbQueries.CreateUserAsync(userRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task SetMetadataStoredAsync(bool isMetadataStored)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            var updateDTO = new UserMetadataFlagUpdateDTO();
            updateDTO.Uid = _firebaseAuthService.GetUid();
            updateDTO.IsMetadataStored = isMetadataStored;

            UserMetadataFlagUpdateDTO? response = new UserMetadataFlagUpdateDTO();

            try
            {
                response = await _firestoreDbQueries.SetMetadataStoredAsync(updateDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task<bool> GetMetadataStoredAsync()
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            var updateDTO = new UserMetadataFlagUpdateDTO();
            updateDTO.Uid = _firebaseAuthService.GetUid();

            UserMetadataFlagUpdateDTO? response = new UserMetadataFlagUpdateDTO();

            try
            {
                response = await _firestoreDbQueries.GetUserMetadataFlagAsync(updateDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");

            return response.IsMetadataStored;
        }
    }
}
