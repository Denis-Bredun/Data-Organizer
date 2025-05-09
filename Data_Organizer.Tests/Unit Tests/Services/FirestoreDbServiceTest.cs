using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.Queries;
using Data_Organizer.Services;
using Moq;

namespace Data_Organizer.Tests.Unit_Tests.Services
{
    public class FirestoreDbServiceTest
    {
        private readonly Mock<IFirebaseAuthService> firebaseAuthService = new Mock<IFirebaseAuthService>();
        private readonly Mock<IFirestoreDbQueries> firestoreDbQueries = new Mock<IFirestoreDbQueries>();
        private readonly Mock<INotificationService> notificationService = new Mock<INotificationService>();
        private readonly Mock<IDeviceServiceDecorator> deviceServiceDecorator = new Mock<IDeviceServiceDecorator>();
        private readonly IMappingService mappingService = new MappingService();

        [Fact]
        public async Task CreateUserAsync_Should_Throw_UnauthorizedAccessException_When_UserNotAuthorized()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(false);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => firestoreDbService.CreateUserAsync(true, new MVVM.Models.Location())
            );
        }

        [Fact]
        public async Task CreateUserAsync_Should_CreateUser_When_Metadata_Is_Stored()
        {
            var uid = Guid.NewGuid().ToString();
            var isMetadataStored = true;
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            firebaseAuthService.Setup(x => x.GetUid())
                .Returns(uid);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.CreateUserAsync(isMetadataStored, new MVVM.Models.Location());

            deviceServiceDecorator.Verify(x => x.GetDeviceInfo());
            firestoreDbQueries.Verify(x => x.CreateUserAsync(It.IsAny<UserRequestDTO>()));
        }

        [Fact]
        public async Task CreateUserAsync_Should_CreateUser_When_Metadata_IsNot_Stored()
        {
            var uid = Guid.NewGuid().ToString();
            var isMetadataStored = false;
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            firebaseAuthService.Setup(x => x.GetUid())
                .Returns(uid);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.CreateUserAsync(isMetadataStored, new MVVM.Models.Location());

            firestoreDbQueries.Verify(x => x.CreateUserAsync(It.IsAny<UserRequestDTO>()));
        }

        [Fact]
        public async Task SetUserMetadataFlagAsync_Should_Throw_UnauthorizedAccessException_When_UserNotAuthorized()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(false);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => firestoreDbService.SetUserMetadataFlagAsync(true)
            );
        }

        [Fact]
        public async Task SetUserMetadataFlagAsync_Should_Call_SetUserMetadataFlagAsync()
        {
            var uid = Guid.NewGuid().ToString();
            var isMetadataStored = true;
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            firebaseAuthService.Setup(x => x.GetUid())
                .Returns(uid);
            firestoreDbQueries.Setup(x => x.SetUserMetadataFlagAsync(It.IsAny<UserMetadataFlagUpdateDTO>()))
                .ReturnsAsync(new UserMetadataFlagUpdateDTO());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.SetUserMetadataFlagAsync(isMetadataStored);

            firestoreDbQueries.Verify(x => x.SetUserMetadataFlagAsync(It.IsAny<UserMetadataFlagUpdateDTO>()));
        }

        [Fact]
        public async Task GetUserMetadataFlagAsync_Should_Throw_UnauthorizedAccessException_When_UserNotAuthorized()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(false);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => firestoreDbService.GetUserMetadataFlagAsync()
            );
        }

        [Fact]
        public async Task GetUserMetadataFlagAsync_Should_Return_Result()
        {
            var uid = Guid.NewGuid().ToString();
            var expected = new UserMetadataFlagUpdateDTO() { IsMetadataStored = true };
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            firebaseAuthService.Setup(x => x.GetUid())
                .Returns(uid);
            firestoreDbQueries.Setup(x => x.GetUserMetadataFlagAsync(It.IsAny<UserMetadataFlagUpdateDTO>()))
                .ReturnsAsync(expected);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            var result = await firestoreDbService.GetUserMetadataFlagAsync();

            firestoreDbQueries.Verify(x => x.GetUserMetadataFlagAsync(It.IsAny<UserMetadataFlagUpdateDTO>()));
            Assert.Equal(expected.IsMetadataStored, result);
        }

        [Fact]
        public async Task GetUsersMetadataDTO_Should_Return_Result()
        {
            var expected = new UsersMetadataDTO()
            {
                Uid = "expectedId",
                CreationDate = DateTime.Now
            };
            firestoreDbQueries.Setup(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()))
                .ReturnsAsync(expected);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            var result = await firestoreDbService.GetUsersMetadataDTO("testId");

            firestoreDbQueries.Verify(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()));
            Assert.NotNull(result);
            Assert.Equal(expected.Uid, result.Uid);
            Assert.Equal(expected.CreationDate, result.CreationDate);
        }

        [Fact]
        public async Task RemoveUserAsync_Should_RemoveUser_When_Metadata_Is_Stored()
        {
            var uid = Guid.NewGuid().ToString();
            var isMetadataStored = true;
            var location = new MVVM.Models.Location() { Latitude = 30, Longitude = 30 };
            var userMetaData = new UsersMetadataDTO()
            {
                Uid = uid,
                CreationDate = DateTime.Now
            };
            firestoreDbQueries.Setup(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()))
                .ReturnsAsync(userMetaData);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            firestoreDbQueries.Setup(x => x.RemoveUserAsync(It.IsAny<UserRequestDTO>()))
                .ReturnsAsync(new UserRequestDTO());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.RemoveUserAsync(uid, isMetadataStored, location);

            firestoreDbQueries.Verify(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()));
            deviceServiceDecorator.Verify(x => x.GetDeviceInfo());
            firestoreDbQueries.Verify(x => x.RemoveUserAsync(It.IsAny<UserRequestDTO>()));

        }

        [Fact]
        public async Task RemoveUserAsync_Should_RemoveUser_When_Metadata_IsNot_Stored()
        {
            var uid = Guid.NewGuid().ToString();
            var isMetadataStored = false;
            var location = new MVVM.Models.Location() { Latitude = 30, Longitude = 30 };
            var userMetaData = new UsersMetadataDTO()
            {
                Uid = uid,
                CreationDate = DateTime.Now
            };
            firestoreDbQueries.Setup(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()))
                .ReturnsAsync(userMetaData);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            firestoreDbQueries.Setup(x => x.RemoveUserAsync(It.IsAny<UserRequestDTO>()))
                .ReturnsAsync(new UserRequestDTO());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.RemoveUserAsync(uid, isMetadataStored, location);

            firestoreDbQueries.Verify(x => x.RemoveUserAsync(It.IsAny<UserRequestDTO>()));
        }

        [Fact]
        public async Task CreateChangePasswordInstance_Should_Throw_UnauthorizedAccessException_When_UserNotAuthorized()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(false);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => firestoreDbService.CreateChangePasswordInstance("oldPassword")
            );
        }

        [Fact]
        public async Task CreateChangePasswordInstance_Should_CreateAccountLoginAsync()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            var uid = Guid.NewGuid().ToString();
            var userMetaData = new UsersMetadataDTO()
            {
                Uid = uid,
                CreationDate = DateTime.Now
            };
            firestoreDbQueries.Setup(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()))
                .ReturnsAsync(userMetaData);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            deviceServiceDecorator.Setup(x => x.GetCurrentLocationAsync())
                .ReturnsAsync(new MVVM.Models.Location() { Latitude = 10, Longitude = 10 });
            firestoreDbQueries.Setup(x => x.CreateChangePasswordAsync(It.IsAny<ChangePasswordRequestDTO>()))
                .ReturnsAsync(new ChangePasswordRequestDTO());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.CreateChangePasswordInstance("oldPassword");

            deviceServiceDecorator.Verify(x => x.GetDeviceInfo());
            firebaseAuthService.Verify(x => x.GetUid());
            deviceServiceDecorator.Verify(x => x.GetCurrentLocationAsync());
            firestoreDbQueries.Verify(x => x.CreateChangePasswordAsync(It.IsAny<ChangePasswordRequestDTO>()));
        }

        [Fact]
        public async Task CreateAccountLoginInstance_Should_Throw_UnauthorizedAccessException_When_UserNotAuthorized()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(false);
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => firestoreDbService.CreateAccountLoginInstance()
            );
        }

        [Fact]
        public async Task CreateAccountLoginInstance_Should_CreateAccountLoginAsync()
        {
            firebaseAuthService.Setup(x => x.IsUserAuthorized())
                .Returns(true);
            var uid = Guid.NewGuid().ToString();
            var userMetaData = new UsersMetadataDTO()
            {
                Uid = uid,
                CreationDate = DateTime.Now
            };
            firestoreDbQueries.Setup(x => x.GetUserMetadataAsync(It.IsAny<UsersMetadataDTO>()))
                .ReturnsAsync(userMetaData);
            deviceServiceDecorator.Setup(x => x.GetDeviceInfo())
                .Returns(new MVVM.Models.DeviceInfoModel());
            deviceServiceDecorator.Setup(x => x.GetCurrentLocationAsync())
                .ReturnsAsync(new MVVM.Models.Location() { Latitude = 10, Longitude = 10 });
            firestoreDbQueries.Setup(x => x.CreateAccountLoginAsync(It.IsAny<AccountLoginRequestDTO>()))
                .ReturnsAsync(new AccountLoginRequestDTO());
            var firestoreDbService = new FirestoreDbService(
                firebaseAuthService.Object,
                firestoreDbQueries.Object,
                notificationService.Object,
                deviceServiceDecorator.Object,
                mappingService);

            await firestoreDbService.CreateAccountLoginInstance();

            deviceServiceDecorator.Verify(x => x.GetDeviceInfo());
            firebaseAuthService.Verify(x => x.GetUid());
            deviceServiceDecorator.Verify(x => x.GetCurrentLocationAsync());
            firestoreDbQueries.Verify(x => x.CreateAccountLoginAsync(It.IsAny<AccountLoginRequestDTO>()));
        }
    }
}