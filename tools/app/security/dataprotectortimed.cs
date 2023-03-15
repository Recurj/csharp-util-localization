namespace RJToolsApp.Security {
    public class DataProtectorTimed : AppProtectorTimed {
        private static readonly byte[] key = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
        private static readonly byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };
        public DataProtectorTimed() : base(key, iv) {

        }
        public static string Encrypt(string data) {
            DataProtectorTimed de = new();
            return de.DoEncrypt(data);
        }
        public static AppProtectedTimedData Decrypt(string data) {
            DataProtectorTimed de = new();
            return de.DoDecrypt(data);
        }
    }
}
