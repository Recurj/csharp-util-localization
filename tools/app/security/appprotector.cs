using System;
using System.IO;
using System.Security.Cryptography;
namespace RJToolsApp.Security {
    public class AppProtector {
        private byte[] Key { get; init; }
        private byte[] IV { get; init; }
        public AppProtector(byte[] key, byte[] iv) {
            Key = key;
            IV = iv;
        }
        public string DoEncrypt(string data) {
            var s = DateTime.UtcNow.ToString("HH:mm:ss");
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(Key, IV))
            using (MemoryStream msEncrypt = new())
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                using (StreamWriter swEncrypt = new(csEncrypt)) {
                    swEncrypt.Write(data);
                    swEncrypt.Write(s);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        public string DoDecrypt(string data) {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(Key, IV))
            using (MemoryStream msDecrypt = new(Convert.FromBase64String(data)))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt)) {
                var s = srDecrypt.ReadToEnd();
                if (s.Length > 8) return s[0..^8];
            }
            return null;
        }
    }
}
