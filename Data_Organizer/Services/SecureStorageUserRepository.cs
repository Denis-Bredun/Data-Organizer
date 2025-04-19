using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using Firebase.Auth;
using Firebase.Auth.Repository;
using System.Text.Json;

namespace Data_Organizer.Services
{
    public class SecureStorageUserRepository : IUserRepository
    {
        private readonly string _appName;
        private readonly IEncryptionService _encryptionService;
        private const string AUTH_DATA_KEY = "firebase_auth_data";

        public SecureStorageUserRepository(string appName, IEncryptionService encryptionService)
        {
            _appName = appName;
            _encryptionService = encryptionService;
        }

        public bool UserExists()
        {
            return !string.IsNullOrEmpty(SecureStorage.Default.GetAsync(GetStorageKey()).Result);
        }

        public (UserInfo userInfo, FirebaseCredential credential) ReadUser()
        {
            try
            {
                string encryptedJson = SecureStorage.Default.GetAsync(GetStorageKey()).Result;

                if (string.IsNullOrEmpty(encryptedJson))
                    return (null, null);

                string decryptedJson = _encryptionService.Decrypt(encryptedJson);

                if (string.IsNullOrEmpty(decryptedJson))
                    return (null, null);

                var userData = JsonSerializer.Deserialize<UserData>(decryptedJson);
                return (userData?.UserInfo, userData?.Credential);
            }
            catch
            {
                return (null, null);
            }
        }

        public void SaveUser(Firebase.Auth.User user)
        {
            if (user == null)
            {
                DeleteUser();
                return;
            }

            var userData = new UserData
            {
                UserInfo = user.Info,
                Credential = user.Credential
            };

            string json = JsonSerializer.Serialize(userData);
            string encryptedJson = _encryptionService.Encrypt(json);
            SecureStorage.Default.SetAsync(GetStorageKey(), encryptedJson).Wait();
        }

        public void DeleteUser()
        {
            SecureStorage.Default.Remove(GetStorageKey());
        }

        private string GetStorageKey() => $"{_appName}_{AUTH_DATA_KEY}";
    }
}