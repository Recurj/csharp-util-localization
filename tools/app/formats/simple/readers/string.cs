namespace RJToolsApp.Formats.Simple {
    public class RJFormatReaderString : IRJFormatReader {
        public string Value { get; set; } = "";
        public bool start(int l) {
            Value = "";
            return true;
        }
        public bool onRead(int f, string v) {
            switch (f) {
                case 1:
                    Value = v;
                    break;
                default:
                    return false;
            }
            return true;
        }

        public bool total(int c) => c == 1;
    }
}