using Data_Organizer.MVVM.Models;
using Firebase.Auth;
using Firebase.Auth.Repository;
using System.Text.Json;

namespace Data_Organizer.Services
{
    public class SecureStorageUserRepository : IUserRepository
    {
        private readonly string _appName;
        private const string AuthDataKey = "firebase_auth_data";

        public SecureStorageUserRepository(string appName)
        {
            _appName = appName;
        }

        public bool UserExists()
        {
            return !string.IsNullOrEmpty(SecureStorage.Default.GetAsync(GetStorageKey()).Result);
        }

        public (UserInfo userInfo, FirebaseCredential credential) ReadUser()
        {
            try
            {
                string json = SecureStorage.Default.GetAsync(GetStorageKey()).Result;
                
                if (string.IsNullOrEmpty(json))
                    return (null, null);
                
                var userData = JsonSerializer.Deserialize<UserData>(json);
                return (userData?.UserInfo, userData?.Credential);
            }
            catch
            {
                return (null, null);
            }
        }

        public void SaveUser(User user)
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
            SecureStorage.Default.SetAsync(GetStorageKey(), json).Wait();
        }

        public void DeleteUser()
        {
            SecureStorage.Default.Remove(GetStorageKey());
        }

        private string GetStorageKey() => $"{_appName}_{AuthDataKey}";
    }    
} 