using System.IO;

namespace RJToolsApp {
    public interface IProcessorFile {
        bool ProcessFile(string fn);
    }
    public class ProcessorCmdFiles {
        private IProcessorFile _process;
        public ProcessorCmdFiles(IProcessorFile process) {
            _process = process;
        }
        public bool WorkUp(string fn) {
            if (fn[0] == '@') {
                RJApplication.Logger.InfoMsg($"Read list of files {fn}");
                return ReadFile(fn[1..]);
            }
            RJApplication.Logger.InfoMsg($"Process file {fn}");
            return _process.ProcessFile(fn);
        }

        public bool ReadFile(string fn) {
            using (FileStream fs = File.OpenRead(fn)) {
                using (var reader = new StreamReader(fs, System.Text.Encoding.UTF8, true)) {
                    reader.Peek();
                    while (!reader.EndOfStream) {
                        if (!WorkUp(reader.ReadLine())) return false;
                    }
                }
            }
            return true;
        }
    }
}
