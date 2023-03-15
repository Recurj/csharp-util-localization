namespace RJToolsApp.Formats.Simple {
    public class RJFormatReaderArrayElement : RJFormatReaderLevel {
        public int Count { get; set; } = 0;
        readonly RJFormatReaderArrayElem _elemParser;
        public RJFormatReaderArrayElement(RJFormatReaderArrayElem parser) {
            _elemParser = parser;
        }
        public override bool start(int l) {
            Count = 0;
            _elemParser.Seq = 0;
            return base.start(l);
        }
        public override bool onRead(int f, string v) {
            _elemParser.Seq++;
            return RJFormatParser.Fields(v, NextLevel, _elemParser) == RemoteAppError.No;
        }

        public override bool total(int c) {
            Count = c;
            return true;
        }
    }


    public class RJFormatReaderArray : RJFormatReaderLevel {
        readonly RJFormatReaderArrayElem _elemParser;
        public int Count { get; set; } = 0;
        public RJFormatReaderArray(RJFormatReaderArrayElem fieldParser) {
            _elemParser = fieldParser;
        }

        public override bool start(int l) {
            Count = 0;
            return base.start(l);
        }
        public override bool onRead(int f, string v) {
            switch (f) {
                case 1: return _parseRecords(v);
                case 2: return int.TryParse(v, out var c) && c == Count;
            }
            return false;
        }

        public override bool total(int c) => c == 2;

        bool _parseRecords(string value) {
            var reader = new RJFormatReaderArrayElement(_elemParser);
            if (RJFormatParser.Fields(value, NextLevel, reader) == RemoteAppError.No) {
                Count=reader.Count;
                return true;
            }
            return false;
        }
    }
}


