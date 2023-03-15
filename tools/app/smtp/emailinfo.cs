using System.Globalization;
using System.Net.Mail;

namespace RJToolsApp.smtp {
    public class EMailInfo : MailAddress {

        public string EMail { get; }
        public string Domain { get; }
        public EMailInfo(string email, bool bLow) : base(email) {
            EMail = bLow ? Address.ToLower(CultureInfo.InvariantCulture) : Address;
            Domain = Host.ToLower(CultureInfo.InvariantCulture);
        }
    }
}
