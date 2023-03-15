using RJAppToolsLog;
using RJToolsApp;
using System.Collections.Generic;
namespace LoaderDbApp {
    class Program {
        class CAppOptions : CRJAppStartup {
            public List<string> Files { get; set; } = new();
            public string Database { get; set; }
            public CAppOptions() : base("import") {
            }
            public override bool OnOption(int ind, string opt, string v) {
                if (opt == "-load") {
                    Files.Add(v);
                    return true;
                }
                else if (opt == "-db") {
                    Database = v;
                    return true;
                }
                return base.OnOption(ind, opt, v);
            }
        }
        static void Main(string[] args) => RJApplication.TryAction(() => {
            CAppOptions startup = new();
            if (CAppOptions.ParseArgs(args, startup)) {
                var cfg = SerilogLogger.SetAppLog(startup, RJApplication.AppProcessName);
                RJApplication.TryMeasure(() => {
                    App app = new();
                    app.SetDatabase(startup.Database);
                    foreach (var val in startup.Files) app.LoadFile(val);
                    app.Done();
                    if (app.HasError) RJApplication.Logger.InfoMsg("Errors count=" + app.Errors);
                });
            }
        });
    }
}
