using System.Collections.Generic;

namespace RJToolsApp.Formats.Simple {
    public class RJFormatReaderHandbook : RJFormatReaderStart {
        public Dictionary<long, string> Handbook { get; init; }
        long _id = 0;
        string _value = "";

        public RJFormatReaderHandbook(Dictionary<long, string> h) {
            Handbook = h;
        }
        public override bool onRead(int f, string v) {
            switch (f) {
                case 1: return long.TryParse(v, out _id);
                case 2: _value = v; return true;
            }
            return false;
        }
        public override bool total(int c) {
            if (c == 2) {
                Handbook.Add(_id, _value);
            }
            return false;
        }
    }
}
