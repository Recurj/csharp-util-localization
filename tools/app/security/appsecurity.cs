using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace RJToolsApp.Security {
    public interface IPolicyPasswordManager {
        (bool, bool) Check(int format, string parameters, string hash, string password);
    }
    public interface IPolicyPassword {
        int GetFormat();
        bool Check(string parameters, string hash, string password);
        (int, string, string) Generate(string password);
    }
    public sealed class AppSecurity {
        public const string PublicHeader = "-----BEGIN PUBLIC KEY-----";
        public const string PublicFooter = "-----END PUBLIC KEY-----";
        private const string _pemDelimiter = "-----";
        private int _saltSize = 16, _hashSize = 20, _hashIter = 10000;
        public static string GetPublicPem(string d) => AppSecurity.PublicHeader + "\n" + d + "\n" + AppSecurity.PublicFooter;
        public AppSecurity(int SaltSize, int HashSize, int HashIter) {
            _saltSize = SaltSize;
            _hashSize = HashSize;
            _hashIter = HashIter;
        }
        public string PasswordGenerate(string password) {
            using (var provider = RandomNumberGenerator.Create()) {
                byte[] salt, hashBytes = new byte[_saltSize + _hashSize];
                provider.GetBytes(salt = new byte[_saltSize]);
                using (var alg = new Rfc2898DeriveBytes(password, salt, _hashIter)) {
                    var hash = alg.GetBytes(_hashSize);
                    Array.Copy(hash, 0, hashBytes, 0, _hashSize);
                    Array.Copy(salt, 0, hashBytes, _hashSize, _saltSize);
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }
        public bool PasswordVerify(string password, string passwordHash) {
            byte[] test, hashBytes = Convert.FromBase64String(passwordHash), salt = new byte[16];
            Array.Copy(hashBytes, _hashSize, salt, 0, _saltSize);
            using (var alg = new Rfc2898DeriveBytes(password, salt, _hashIter)) {
                test = alg.GetBytes(_hashSize);
                for (int i = 0; i < _hashSize; i++)
                    if (hashBytes[i] != test[i])
                        return false;
                return true;
            }
        }
        public static string CodeGenerate(int max) {
            using (var rg = RandomNumberGenerator.Create()) {
                byte[] value = new byte[8];
                rg.GetBytes(value);
                var s = Convert.ToBase64String(value);
                return (s.Length > max) ? s.Substring(0, max) : s;
            }
        }
        public static string ReadPassword() {
            using (var pwd = new SecureString()) {
                bool bSkip = false;
                while (true) {
                    ConsoleKeyInfo i = Console.ReadKey(true);
                    if (bSkip) {
                        bSkip = false;
                        continue;
                    }
                    if (i.Key == ConsoleKey.Enter)
                        return new System.Net.NetworkCredential(String.Empty, pwd).Password;
                    if (i.Key == ConsoleKey.Escape)
                        return null;
                    if (i.Key == ConsoleKey.Backspace) {
                        if (pwd.Length > 0) {
                            pwd.RemoveAt(pwd.Length - 1);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                            Console.Write("\b \b");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                        }
                    }
                    else if (i.KeyChar != '\u0000') {
                        pwd.AppendChar(i.KeyChar);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                        Console.Write("*");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    }
                    else
                        bSkip = true;
                }
            }
        }
        //        public static System.Security.Cryptography.X509Certificates.X509Certificate X509FromPemFile(string fn) {
        //            string s = File.ReadAllText(fn);
        //#pragma warning disable CA1307 // Specify StringComparison
        //            int i = s.IndexOf(_pemDelimiter, 6);
        //            if (i > 0) {
        //                int f = i + _pemDelimiter.Length;
        //                i = s.IndexOf(_pemDelimiter, f);
        //                if (i > 0) {
        //                    X509CertificateParser parser = new X509CertificateParser();
        //                    byte[] buffer = Convert.FromBase64String(s.Substring(f, i - f));
        //                    Org.BouncyCastle.X509.X509Certificate cert = parser.ReadCertificate(buffer);
        //                    return new System.Security.Cryptography.X509Certificates.X509Certificate(cert.GetEncoded());
        //                }
        //            };
        //#pragma warning restore CA1307 // Specify StringComparison
        //            return null;
        //        }
        //        public static RsaPrivateCrtKeyParameters PrivateKeyFromPemFile(String fn) {
        //            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(fn))) {
        //                var obj = new PemReader(privateKeyTextReader).ReadObject();
        //                return (RsaPrivateCrtKeyParameters)obj;
        //            }
        //        }
        //        public static RsaKeyParameters PublicKeyFromPemString(String key) {
        //            var obj = new PemReader(new StringReader(key)).ReadObject();
        //            return (RsaKeyParameters)obj;
        //        }
        //        static public string RsaEncryptPublic(RsaKeyParameters ps, byte[] plain) {
        //            OaepEncoding eng = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha256Digest(), null);
        //            eng.Init(true, ps);
        //            var enc = eng.ProcessBlock(plain, 0, plain.Length);
        //            return Convert.ToBase64String(enc);

        //            //int length = plain.Length;
        //            //int blockSize = eng.GetInputBlockSize();
        //            //List<byte> cipherTextBytes = new List<byte>();
        //            //for (int chunkPosition = 0;chunkPosition < length;chunkPosition += blockSize) {
        //            //    chunkSize = Math.Min(blockSize, length - chunkPosition);
        //            //    cipherTextBytes.AddRange(eng.ProcessBlock(plain, chunkPosition, chunkSize));
        //            //}
        //            //return Convert.ToBase64String(cipherTextBytes.ToArray());
        //        }
        //        static public string RsaEncryptWithPublic(string key, byte[] plain) {
        //            //         var eng = new Pkcs1Encoding(new RsaEngine());
        //            OaepEncoding eng = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha256Digest(), null);
        //            using (var txtreader = new StringReader(key)) {
        //                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
        //                eng.Init(true, keyParameter);
        //                var enc = eng.ProcessBlock(plain, 0, plain.Length);
        //                return Convert.ToBase64String(enc);
        //            }
        //        }
        //    }
    }
}
