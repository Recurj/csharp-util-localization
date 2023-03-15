using System;
using System.Globalization;

namespace RJToolsApp {
    static public class DTimeProvider {
        private const string c_fmtDateTime = "yyyy-MM-dd HH:mm:ss";
        private const string c_fmtDate = "yyyy-MM-dd";
        public static string GetCurrent() => FmtDateTime(DateTime.UtcNow);
        public static string FmtDateTime(DateTime dt) => dt.ToString(c_fmtDateTime);
        public static string FmtDate(DateTime dt) => dt.ToString(c_fmtDate);
        public static string DTByCulture(string c, string s, string f) =>
            RJApplication.TryReturn<string>(s, () => {
                CultureInfo ci = new(c);
                DateTime dt = DateTime.ParseExact(s, f, null);
                return dt.ToString(ci);
            });
        public static DateTime DTByDefault(string s) =>
            DateTime.ParseExact(s, c_fmtDateTime, null);
    }
}
