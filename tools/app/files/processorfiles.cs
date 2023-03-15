using System.IO;

namespace RJToolsApp {
    public interface IProcessorFiles {
        bool ProcessContinue();
        bool NeedProcessFile(string fn);
        bool ProcessFile(string fn);
        void ProcessException(string fn);
        string GetFilesMasks(string fn);

    }
    public class ProcessorFiles {
        private readonly IProcessorFiles _processor;
        public int Total { get; set; } = 0;
        public int Ignored { get; set; } = 0;
        public int Failured { get; set; } = 0;

        public ProcessorFiles(IProcessorFiles processor) {
            _processor = processor;
        }
        public void ProcessPath(string path) {
            if (_processor.ProcessContinue()) {
                if (File.Exists(path)) ProcessFile(path);
                else if (Directory.Exists(path)) ProcessDirectory(path);
                else RJApplication.Logger.ErrorMsg($"{path} is not a valid file or directory.");
            }
        }
        public void ProcessFiles(string[] args) {
            foreach (string path in args) ProcessPath(path);
        }
        void ProcessDirectory(string dir) {
            // Process the list of files found in the directory.
            string s = _processor.GetFilesMasks(dir);
            string[] w = (s != null) ? Directory.GetFiles(dir, s) : Directory.GetFiles(dir);
            foreach (string fn in w) ProcessFile(fn);
            w = Directory.GetDirectories(dir);
            foreach (string dn in w) ProcessDirectory(dn);
        }
        void ProcessFile(string fn) => RJApplication.TryActionError(() => {
            Total++;
            if (_processor.NeedProcessFile(fn)) {
                if (!_processor.ProcessFile(fn)) Failured++;
            }
            else Ignored++;
        }, () => {
            _processor.ProcessException(fn);
            Failured++;
        });
    }
}
