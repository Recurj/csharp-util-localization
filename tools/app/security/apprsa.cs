using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RJToolsApp.Security {
    public class AppRSA {
        private RSA _rsa;
        public long Index { get; init; }
        public AppRSA(long ind) {
            Index = ind;
        }
        public bool Create(int s = 4096) => RJApplication.TryResult(() => {
            _rsa = RSA.Create(s);
            return true;
        });
        public bool Import(string fn) => RJApplication.TryResult(() => {
            _rsa = RSA.Create();
            _rsa.ImportFromPem(File.ReadAllText(fn).ToCharArray());
            return true;
        });
        public string Export() => RJApplication.TryReturn<string>(() =>
            Convert.ToBase64String(_rsa.ExportSubjectPublicKeyInfo()));
        public string EncryptBase64(string s) =>
            Convert.ToBase64String(Encrypt(s));
        public string DecryptBase64(string s) =>
            Decrypt(Convert.FromBase64String(s));
        public byte[] DecryptBytes(string s) =>
            _rsa.Decrypt(Convert.FromBase64String(s), RSAEncryptionPadding.Pkcs1);
        public byte[] Encrypt(string s) =>
            _rsa.Encrypt(Encoding.UTF8.GetBytes(s), RSAEncryptionPadding.Pkcs1);
        public string Decrypt(byte[] cipherText) =>
            Encoding.UTF8.GetString(_rsa.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1));
        public string EncryptBase64(byte[] key, byte[] iv) {
            byte[] data = new byte[key.Length + iv.Length];
            Array.Copy(key, 0, data, 0, key.Length);
            Array.Copy(iv, 0, data, key.Length, iv.Length);
            return Convert.ToBase64String(_rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1));
        }
    }
}
