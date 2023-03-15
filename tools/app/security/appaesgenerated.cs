using System;
using System.IO;
using System.Security.Cryptography;

namespace RJToolsApp.Security {
    public class AppAESGenerated {
        private Aes _aes;
        public byte[] GetIV() => _aes.IV;
        public byte[] GetKey() => _aes.Key;
        public AppAESGenerated(int s = 256) {
            _aes = Aes.Create();
            _aes.KeySize = s;
            _aes.BlockSize = 128;
            _aes.Padding = PaddingMode.PKCS7;
            _aes.GenerateIV();
            _aes.GenerateKey();
        }
        public string EncryptBase64(string data) {
            using ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt)) swEncrypt.Write(data);
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }
}
