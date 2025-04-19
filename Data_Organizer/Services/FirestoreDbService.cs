using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
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

        public async Task CreateUserAsync(bool isMetadataStored, Data_Organizer.MVVM.Models.Location location)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            var uid = _firebaseAuthService.GetUid();
            var dateTimeNow = DateTime.Now;

            UserRequestDTO userRequestDTO = new()
            {
                UserDTO = new UserDTO
                {
                    Uid = uid,
                    IsMetadataStored = isMetadataStored,
                    IsDeleted = false
                }
            };

            if (isMetadataStored)
            {
                var deviceInfo = _deviceServiceDecorator.GetDeviceInfo();

                userRequestDTO.UsersMetadataDTO = new UsersMetadataDTO
                {
                    CreationDate = dateTimeNow,
                    CreationLocation = location
                };

                userRequestDTO.CreationDevice = deviceInfo;
            }

            UserRequestDTO? response;

            try
            {
                response = await _firestoreDbQueries.CreateUserAsync(userRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response?.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task SetUserMetadataFlagAsync(bool isMetadataStored)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            var updateDTO = new UserMetadataFlagUpdateDTO();
            updateDTO.Uid = _firebaseAuthService.GetUid();
            updateDTO.IsMetadataStored = isMetadataStored;

            UserMetadataFlagUpdateDTO? response = new UserMetadataFlagUpdateDTO();

            try
            {
                response = await _firestoreDbQueries.SetUserMetadataFlagAsync(updateDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task<bool> GetUserMetadataFlagAsync()
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
                return false;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");

            return response.IsMetadataStored;
        }

        public async Task<UsersMetadataDTO> GetUsersMetadataDTO(string uid)
        {
            var metadataDTO = new UsersMetadataDTO();
            metadataDTO.Uid = uid;

            UsersMetadataDTO? response = new UsersMetadataDTO();

            try
            {
                response = await _firestoreDbQueries.GetUserMetadataAsync(metadataDTO);
            }
            catch (Exception ex)
            {
                //await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return new UsersMetadataDTO();
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                //throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
                return new UsersMetadataDTO();
            }

            return response;
        }

        public async Task RemoveUserAsync(string uid, bool isMetadataStored, Data_Organizer.MVVM.Models.Location location)
        {
            UserRequestDTO userRequestDTO = new();

            var userDTO = new UserDTO();
            userDTO.Uid = uid;
            userDTO.IsMetadataStored = isMetadataStored;

            userRequestDTO.UserDTO = userDTO;

            if (userDTO.IsMetadataStored)
            {
                var DTOofExistingMetadata = await GetUsersMetadataDTO(uid);

                var deviceInfo = _deviceServiceDecorator.GetDeviceInfo();
                var dateTimeNow = DateTime.Now;

                var metadata = new UsersMetadata()
                {
                    DeletionDate = dateTimeNow,
                    DeletionLocation = location
                };

                userRequestDTO.UsersMetadataDTO = _mappingService.MapMetadata(metadata);

                userRequestDTO.DeletionDevice = deviceInfo;
                userRequestDTO.UsersMetadataDTO.CreationDeviceId = DTOofExistingMetadata.CreationDeviceId;
                userRequestDTO.UsersMetadataDTO.CreationDate = DTOofExistingMetadata.CreationDate;
                userRequestDTO.UsersMetadataDTO.CreationLocation = DTOofExistingMetadata.CreationLocation;
            }

            UserRequestDTO? response = new UserRequestDTO();

            try
            {
                response = await _firestoreDbQueries.RemoveUserAsync(userRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task CreateChangePasswordInstance(string oldPassword)
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            ChangePasswordRequestDTO changePasswordRequestDTO = new ChangePasswordRequestDTO();

            changePasswordRequestDTO.DeviceInfo = _deviceServiceDecorator.GetDeviceInfo();
            changePasswordRequestDTO.Uid = _firebaseAuthService.GetUid();

            ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO();
            changePasswordDTO.OldPassword = oldPassword;
            changePasswordDTO.Date = DateTime.Now;
            changePasswordDTO.Location = await _deviceServiceDecorator.GetCurrentLocationAsync();

            changePasswordRequestDTO.ChangePasswordDTO = changePasswordDTO;

            ChangePasswordRequestDTO? response = new ChangePasswordRequestDTO();

            try
            {
                response = await _firestoreDbQueries.CreateChangePasswordAsync(changePasswordRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task CreateAccountLoginInstance()
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            AccountLoginRequestDTO accountLoginRequestDTO = new AccountLoginRequestDTO();

            accountLoginRequestDTO.Uid = _firebaseAuthService.GetUid();
            accountLoginRequestDTO.DeviceInfo = _deviceServiceDecorator.GetDeviceInfo();

            AccountLoginDTO accountLoginDTO = new AccountLoginDTO();

            accountLoginDTO.Date = DateTime.Now;
            accountLoginDTO.Location = await _deviceServiceDecorator.GetCurrentLocationAsync();

            accountLoginRequestDTO.AccountLoginDTO = accountLoginDTO;

            AccountLoginRequestDTO? response = new AccountLoginRequestDTO();

            try
            {
                response = await _firestoreDbQueries.CreateAccountLoginAsync(accountLoginRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }

        public async Task CreateAccountLogoutInstance()
        {
            if (!_firebaseAuthService.IsUserAuthorized())
                throw new UnauthorizedAccessException("Користувач незареєстрований!");

            AccountLogoutRequestDTO accountLogoutRequestDTO = new AccountLogoutRequestDTO();

            accountLogoutRequestDTO.Uid = _firebaseAuthService.GetUid();
            accountLogoutRequestDTO.DeviceInfo = _deviceServiceDecorator.GetDeviceInfo();

            AccountLogoutDTO accountLogoutDTO = new AccountLogoutDTO();

            accountLogoutDTO.Date = DateTime.Now;
            accountLogoutDTO.Location = await _deviceServiceDecorator.GetCurrentLocationAsync();

            accountLogoutRequestDTO.AccountLogoutDTO = accountLogoutDTO;

            AccountLogoutRequestDTO? response = new AccountLogoutRequestDTO();

            try
            {
                response = await _firestoreDbQueries.CreateAccountLogoutAsync(accountLogoutRequestDTO);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка при запиті до бази даних: {ex.Message}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(response.Error))
                throw new Exception($"Помилка при запиті до бази даних: {response.Error}");
        }
    }
}
