using System;

namespace RJToolsApp {
    public class AppTextReader {
        protected string _orig = "";
        public int Offset { get; private set; }
        public int Last { get; private set; }
        public int Line { get; private set; }
        public int Position { get; private set; }
        public bool HasTail { get => Offset < Last; }
        public char Current { get => _orig[Offset]; }
        public AppTextReader() {
        }
        public void Reset(string orig) {
            _orig = orig;
            Offset = 0;
            Last = orig.Length;
            Line = 1;
            Position = 1;
            skipSpaces();
        }
        public string GetLineCheck() => HasTail ? GetLine() : null;
        public string GetLine() {
            bool bEol = true;
            int l, t = Offset;
            while (notEOLChars()) {
                Offset++;
                if (!HasTail) {
                    bEol = false;
                    break;
                }
            }
            l = Offset;
            if (bEol) moveEOLChars();
            return _orig[t..l];
        }
        public string GetWordCheck() => HasTail ? GetWord() : null;
        public string GetWord() {
            bool bEol = false;
            int l, t = Offset;
            do {
                if (!notEOLChars()) { bEol = true; break; }
                if (IsSeparator()) break;
                Offset++;
            }
            while (HasTail);
            l = Offset;
            if (bEol) moveEOLChars();
            else skipSpaces();
            return _orig[t..l];
        }
        public string GetBlockCheck(char open, char close) => HasTail ? GetBlock(open, close) : null;
        public string GetBlock(char open, char close) {
            if (Current == open) {
                char ch;
                int t = Offset, c = 1;
                Offset++;
                while (HasTail) {
                    ch = Current;
                    Offset++;
                    if (ch == open) c++;
                    else if (ch == close) {
                        c--;
                        if (c == 0) return _orig[t..Offset];
                    }
                }
            }
            return null;
        }
        public string FindCheck(char open) => HasTail ? Find(open) : null;
        public string Find(char open) {
            int t = Offset;
            while (Current != open) {
                Offset++;
                if (!HasTail) return null;
            }
            return _orig[t..Offset];
        }
        virtual public bool IsSeparator() => Current <= 0x20 || Char.IsWhiteSpace(Current);
        public AppTextReader Skip() {
            while (HasTail && (!notEOLChars() || IsSeparator())) {
                Offset++;
                Position++;
            }
            return this;
        }
        public AppTextReader Move(int l) {
            Offset = l;
            return this;
        }
        bool notEOLChars() => Current != 0x0D && Current != 0x0A;
        void moveEOLChars() {
            if (Current == 0x0D) {
                Offset++;
                if (HasTail && Current == 0x0A) Offset++;
            }
            else Offset++;
            Line++;
            Position = 1;
            skipSpaces();
        }
        void skipSpaces() {
            while (HasTail && notEOLChars() && IsSeparator()) {
                Offset++;
                Position++;
            }
        }
    }
}