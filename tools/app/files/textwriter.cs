using System;
using System.IO;
using System.Text;

namespace RJToolsApp {
    public class AppTextWriter : IDisposable {
        private FileStream _file = null;
        protected TextWriter _text = null;
        public AppTextWriter() {

        }
        public void Prepare(string fn, Encoding enc) {
            _file = File.Create(fn);
            if (_file == null) throw new InvalidOperationException("Error::Couldn't create the file " + fn);
            _text = new StreamWriter(_file, enc);
        }
        public void Close() {
            _text.Close();
        }
        public void Dispose() {
            if (_text != null) {
                _text.Dispose();
                _text = null;
            }
            else if (_file != null) {
                _file.Dispose();

            }
            _file = null;

            GC.SuppressFinalize(this);
        }
        public TextWriter GetWriter() => _text;
        public static string Tabs(int level) => new String('\t', level);
    }

}
