using System;

namespace RJToolsApp {
    public class AppConfig {
        public static int GetValue(String s, int d) {
            if (s != null && int.TryParse(s, out int v)) return v;
            return d;
        }
    }
}
