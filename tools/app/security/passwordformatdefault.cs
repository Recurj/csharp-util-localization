using System.Text.Json;
using System.Text.Json.Serialization;

namespace RJToolsApp.Security {

    public class PasswordFormatDefault : IPolicyPassword {
        public const int Id = 1;
        public PasswordFormatDefault() {
            SaltSize = 16;
            HashSize = 20;
            HashIter = 10000;
        }
        public int GetFormat() => Id;

        public bool Check(string parameters, string hash, string password) {
            var a = JsonSerializer.Deserialize<PasswordFormatDefault>(parameters);
            AppSecurity passwordHash = new AppSecurity(a.SaltSize, a.HashSize, a.HashIter);
            return passwordHash.PasswordVerify(password, hash);
        }
        public (int, string, string) Generate(string password) {
            AppSecurity passwordHash = new AppSecurity(SaltSize, HashSize, HashIter);
            return (Id, JsonSerializer.Serialize(this), passwordHash.PasswordGenerate(password));

        }
        [JsonPropertyNameAttribute("f1")]
        public int SaltSize { get; set; }
        [JsonPropertyNameAttribute("f2")]
        public int HashSize { get; set; }
        [JsonPropertyNameAttribute("f3")]
        public int HashIter { get; set; }
    }
    public class PasswordFormatDefaultManager : IPolicyPasswordManager {
        public (bool, bool) Check(int format, string parameters, string hash, string password) {
            if (format == PasswordFormatDefault.Id) {
                PasswordFormatDefault p = new PasswordFormatDefault();
                return (true, p.Check(parameters, hash, password));
            }
            return (false, false);
        }
    }
}
