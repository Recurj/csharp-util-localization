using Microsoft.Extensions.Configuration;
using RJToolsApp;
using Serilog;
using System;
using System.IO;
using System.Text;
namespace RJAppToolsLog {

    public class SerilogLogger : IAppLogger {
        public void InfoMsg(string s) {
            Log.Information(s);
        }
        public void ErrorMsg(string s) {
            Log.Error($"Error::{s}");
        }
        public void Exception(Exception ex) {
            Log.Error(ex, "Exception");
        }
        public void Exception(Exception ex, string context) {
            Log.Error(ex, "Exception::" + context);
        }
        public static IConfigurationRoot SetAppLog(CRJAppStartup startup,string name) {
            StringBuilder sb = new(1024);
            IConfigurationRoot config = startup.GetAppCfg(name);
            sb.Append(startup.AppPathLog).Append(Path.DirectorySeparatorChar).Append(RJApplication.AppProcessName).Append(".log");
            Log.Logger = new LoggerConfiguration().
                ReadFrom.Configuration(config).
                WriteTo.File(sb.ToString()).
                CreateLogger();
            RJApplication.Logger=new SerilogLogger();
            return config;
        }
    }
}
