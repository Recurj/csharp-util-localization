using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace RJToolsApp.Security {
    public class AppProtectedTimedData {
        public DateTime DTime { get; init; }
        public String Value { get; init; }

        public AppProtectedTimedData(string value) {
            DTime = DateTime.UtcNow;
            Value = value;
        }
        public static AppProtectedTimedData Parsed(string s) => JsonSerializer.Deserialize<AppProtectedTimedData>(s);
        public static string Plain(string s) => JsonSerializer.Serialize(new AppProtectedTimedData(s));
    }
    public class AppProtectorTimed {
        private byte[] Key { get; init; }
        private byte[] IV { get; init; }
        public AppProtectorTimed(byte[] key, byte[] iv) {
            Key = key;
            IV = iv;
        }
        public string DoEncrypt(string data) {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(Key, IV))
            using (MemoryStream msEncrypt = new())
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                using (StreamWriter swEncrypt = new(csEncrypt)) {
                    swEncrypt.Write(AppProtectedTimedData.Plain(data));
                }
                return Convert.ToHexString(msEncrypt.ToArray());
            }
        }
        public AppProtectedTimedData DoDecrypt(string data) {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(Key, IV))
            using (MemoryStream msDecrypt = new(Convert.FromHexString(data)))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt)) {
                return AppProtectedTimedData.Parsed(srDecrypt.ReadToEnd());
            }
#pragma warning disable CS0162 // Unreachable code detected
            return null;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
