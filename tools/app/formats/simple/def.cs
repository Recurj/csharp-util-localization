using System;
using System.Collections.Generic;

namespace RJToolsApp.Formats.Simple {
    public class RJFormat {
        public readonly static string OK = "+\n0";
        public readonly static string Failure = "-\n0";
        public readonly static List<String> Separators = new() { "\n0", "\n1", "\n2", "\n3", "\n4", "\n5", "\n6", "\n7", "\n8", "\n9" };
        public static string Data(int v) => v.ToString() + Separators[0];
        public static string Data(long v) => v.ToString() + Separators[0];
    }
    public interface IRJFormatReader {
        bool start(int l);
        bool onRead(int f, String v);
        bool total(int c);
    }

    public abstract class RJFormatReaderStart : IRJFormatReader {
        public virtual bool start(int l) => true;
        public abstract bool onRead(int f, string v);

        public abstract bool total(int c);
    }
    public abstract class RJFormatReader : RJFormatReaderStart {
        readonly int _fields;
        public RJFormatReader(int f) {
            _fields = f;
        }
        public override bool total(int c) => c == _fields;
    }

    public abstract class RJFormatReaderLevel : IRJFormatReader {
        int _level = 0;
        public int NextLevel => _level + 1;
        public virtual bool start(int l) {
            _level = l;
            return true;
        }
        public abstract bool onRead(int f, string v);

        public abstract bool total(int c);
    }
    public abstract class RJFormatReaderArrayElem : IRJFormatReader {
        public int Seq { get; set; }
        public abstract bool start(int l);
        public abstract bool onRead(int f, String v);
        public abstract bool total(int c);
    }
    public abstract class RJFormatReaderArrayElemStart : RJFormatReaderArrayElem {
        public override bool start(int l) => true;
    }
}
