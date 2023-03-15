using System;
using System.IO;
using System.Security.Cryptography;

namespace RJToolsApp.Security {
    public class AppAES {
        private Aes _aes { get; init; }
        private byte[] Key { get; init; }
        private byte[] IV { get; init; }
        public byte[] GetIV() => IV;
        public byte[] GetKey() => Key;
        public AppAES(byte[] data, int s = 256) {
            int off = s / 8;
            _aes = Aes.Create();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.KeySize = s;
            _aes.BlockSize = 128;
            Key = new byte[32];
            IV = new byte[16];
            Array.Copy(data, 0, Key, 0, off);
            Array.Copy(data, off, IV, 0, 16);
        }
        public string EncryptBase64(string data) {
            using (ICryptoTransform encryptor = _aes.CreateEncryptor(Key, IV))
            using (MemoryStream msEncrypt = new())
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                using (StreamWriter swEncrypt = new(csEncrypt)) swEncrypt.Write(data);
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        public string DecryptBase64(string data) {
            using (ICryptoTransform decryptor = _aes.CreateDecryptor(Key, IV))
            using (MemoryStream msDecrypt = new(Convert.FromBase64String(data)))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt)) return srDecrypt.ReadToEnd();
        }
    }
}
