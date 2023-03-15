namespace RJToolsApp.Formats.Simple {
    public class RJFormatReaderEmpty : RJFormatReader {
        public RJFormatReaderEmpty() : base(0) { }
        public override bool onRead(int f, string v) => false;
    }
}