using Data_Organizer.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Data_Organizer.Services
{
    public class EncryptionService : IEncryptionService
    {
        private const string SALT_KEY = "device_salt_key";
        private const int KEY_SIZE = 256;
        private const int DERIVATION_ITERATIONS = 10000;
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(ILogger<EncryptionService> logger)
        {
            _logger = logger;
        }

        public string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    _logger?.LogWarning("Attempt to encrypt null or empty string");
                    return null;
                }

                _logger?.LogDebug("Starting encryption process");
                var saltBytes = GetOrCreateSalt();
                var ivBytes = GetRandomBytes(16);
                
                using var key = new Rfc2898DeriveBytes(GetDeviceSpecificKey(), saltBytes, DERIVATION_ITERATIONS, HashAlgorithmName.SHA256);
                using var aes = Aes.Create();
                aes.KeySize = KEY_SIZE;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = ivBytes;
                
                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                
                var cipherTextBytes = ms.ToArray();
                var result = new byte[ivBytes.Length + cipherTextBytes.Length];
                Buffer.BlockCopy(ivBytes, 0, result, 0, ivBytes.Length);
                Buffer.BlockCopy(cipherTextBytes, 0, result, ivBytes.Length, cipherTextBytes.Length);
                
                var base64Result = Convert.ToBase64String(result);
                _logger?.LogDebug("Encryption completed successfully");
                return base64Result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during encryption: {Message}", ex.Message);
                return null;
            }
        }
        
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                _logger?.LogWarning("Attempt to decrypt null or empty string");
                return null;
            }
                
            try
            {
                _logger?.LogDebug("Starting decryption process");
                var saltBytes = GetOrCreateSalt();
                var allBytes = Convert.FromBase64String(cipherText);
                
                var ivBytes = new byte[16];
                var cipherTextBytes = new byte[allBytes.Length - ivBytes.Length];
                
                Buffer.BlockCopy(allBytes, 0, ivBytes, 0, ivBytes.Length);
                Buffer.BlockCopy(allBytes, ivBytes.Length, cipherTextBytes, 0, cipherTextBytes.Length);
                
                using var key = new Rfc2898DeriveBytes(GetDeviceSpecificKey(), saltBytes, DERIVATION_ITERATIONS, HashAlgorithmName.SHA256);
                using var aes = Aes.Create();
                aes.KeySize = KEY_SIZE;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = ivBytes;
                
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(cipherTextBytes);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                
                var decryptedText = sr.ReadToEnd();
                _logger?.LogDebug("Decryption completed successfully");
                return decryptedText;
            }
            catch (CryptographicException ex)
            {
                _logger?.LogWarning(ex, "Decryption failed - possibly corrupted data or wrong key: {Message}", ex.Message);
                return null;
            }
            catch (FormatException ex)
            {
                _logger?.LogWarning(ex, "Invalid format for decryption: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error during decryption: {Message}", ex.Message);
                return null;
            }
        }
        
        private byte[] GetOrCreateSalt()
        {
            try
            {
                var storedSalt = SecureStorage.Default.GetAsync(SALT_KEY).Result;
                
                if (string.IsNullOrEmpty(storedSalt))
                {
                    _logger?.LogInformation("Creating new salt");
                    var newSalt = GetRandomBytes(32);
                    var saltBase64 = Convert.ToBase64String(newSalt);
                    SecureStorage.Default.SetAsync(SALT_KEY, saltBase64).Wait();
                    return newSalt;
                }
                
                _logger?.LogDebug("Retrieved existing salt");
                return Convert.FromBase64String(storedSalt);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error accessing salt: {Message}", ex.Message);
                return Encoding.UTF8.GetBytes(GetDeviceSpecificKey() + SALT_KEY);
            }
        }
        
        private byte[] GetRandomBytes(int length)
        {
            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return randomBytes;
        }
        
        private string GetDeviceSpecificKey()
        {
            return $"{DeviceInfo.Current.Manufacturer}_{DeviceInfo.Current.Model}_{DeviceInfo.Current.Name}_{DeviceInfo.Current.VersionString}_DataOrganizer";
        }
    }
} 