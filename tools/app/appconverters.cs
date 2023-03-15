namespace RJToolsApp.Converters {
    static public class AppConverters {
        static public int HexDigit(byte b) {
            if (b >= 0x30 && b <= 0x39) return (b - 0x30);
            else if (b >= 0x61 && b <= 0x66) return (b - 0x61 + 10);
            else if (b >= 0x41 && b <= 0x46) return (b - 0x41 + 10);
            return -1;
        }
    }
}
