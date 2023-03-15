using System;
using System.Security.Cryptography;
using System.Text;

namespace RJToolsApp.Security {
    public class AppRSAClient {
        private RSACryptoServiceProvider _rsa;
        public AppRSAClient(string rsaKey, int keyLengthBits = 2048) {
            _rsa = new RSACryptoServiceProvider();
            Load(rsaKey, keyLengthBits);
        }
        void Load(string rsaKey, int keyLengthBits) {
            byte[] publicKeyBytes = Convert.FromBase64String(rsaKey);
            byte[] exponent = new byte[3];
            byte[] modulus = new byte[keyLengthBits / 8];
            Array.Copy(publicKeyBytes, publicKeyBytes.Length - exponent.Length, exponent, 0, exponent.Length);
            Array.Copy(publicKeyBytes, publicKeyBytes.Length - exponent.Length - 2 - modulus.Length, modulus, 0, modulus.Length);
            RSAParameters rsaKeyInfo = _rsa.ExportParameters(false);
            rsaKeyInfo.Modulus = modulus;
            rsaKeyInfo.Exponent = exponent;
            _rsa.ImportParameters(rsaKeyInfo);
        }
        public string EncryptBase64(byte[] data) =>
            Convert.ToBase64String(_rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1));
        public string EncryptBase64(string s) =>
            EncryptBase64(Encoding.UTF8.GetBytes(s));

        public string EncryptBase64(byte[] key, byte[] iv) {
            byte[] data = new byte[key.Length + iv.Length];
            Array.Copy(key, 0, data, 0, key.Length);
            Array.Copy(iv, 0, data, key.Length, iv.Length);
            return EncryptBase64(data);
        }
    }
}
