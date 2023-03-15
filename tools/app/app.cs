using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace RJToolsApp {
    public interface IAppLogger {
        void InfoMsg(string s);
        void ErrorMsg(string s);
        void Exception(Exception ex);

    }
    public interface IAppCmdLine {
        bool OnValue(int ind, string v);
        bool OnOption(int ind, string opt, string v);
        bool OnOptionError(int ind, string v);
        bool OnValueCount(int ind);
    }
    public class CRJAppStartup : IAppCmdLine {
        public string AppPathLog;
        public string AppPathCfg;
        public string AppPart;
        private List<String> _values;
        public CRJAppStartup(string part) {
            var path = RJApplication.GetAppPathBase();
            AppPart = part;
            AppPathCfg = GetAppPathCfg(path);
            AppPathLog = GetAppPathLog(path, part);
        }
        public List<String> GetValues() {
            if (_values == null)
                _values = new();
            return _values;
        }
        public virtual bool OnValue(int ind, string v) {
            GetValues().Add(v);
            return true;
        }
        public virtual bool OnOption(int ind, string opt, string v) {
            if (opt == "-work") {
                AppPathCfg = GetAppPathCfg(v);
                AppPathLog = GetAppPathLog(v, AppPart);
            }
            else
                Console.WriteLine("Unexpected option {opt} with value {v}");
            return true;
        }
        public virtual bool OnOptionError(int ind, string opt) {
            Console.WriteLine("Option {opt} without value");
            return false;
        }
        public virtual bool OnValueCount(int ind) {
            return true;
        }
        // previously name==RJApplication.AppProcessName
        public IConfigurationRoot GetAppCfg(String name, bool opt = false) =>
            new ConfigurationBuilder().AddJsonFile(Path.Combine(AppPathCfg, name) + ".json", optional: opt).Build();
        public static bool ParseArgs(string[] args, IAppCmdLine parser) {
            int opts = 0, vals = 0, i = 0, l = args.Length;
            int ind;
            while (i < l) {
                ind = i;
                i++;
                if (args[ind][0] == '-') {
                    opts++;
                    if (i < l) {
                        if (!parser.OnOption(opts, args[ind], args[i]))
                            return false;
                        i++;
                    }
                    else
                        return parser.OnOptionError(opts, args[ind]);
                }
                else {
                    vals++;
                    if (!parser.OnValue(opts, args[ind]))
                        return false;
                }
            }
            return parser.OnValueCount(vals);
        }
#if DEBUG
        static string GetAppPathCfg(string path) => Path.Combine(path, "cfgs", "debug");
        static string GetAppPathLog(string path, string part) => Path.Combine(path, "logs", "debug", part);
#else
        static string GetAppPathCfg(string path) => Path.Combine(path, "cfgs");
        static string GetAppPathLog(string path, string part) => Path.Combine(path, "logs",part);
#endif
        public static void BadCmdArgs(string s) {
            Console.WriteLine($"Usage {System.Reflection.Assembly.GetExecutingAssembly().Location} {s}");
        }
    }
    public class RJApplication {
        protected static IAppLogger _appLogger;
        public static string AppProcessName => System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        public static string GetAppPathBase() {
            string s = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;
            return Directory.GetParent(s).FullName;
        }
        public static IAppLogger Logger { get => _appLogger; set { _appLogger = value; } }
        public static bool TryAction(Action act) {
            try {
                act();
                return true;
            }
            catch (Exception e) {
                AppException(e);
            }
            return false;
        }
        public static bool TryActionError(Action act, Action err) {
            try {
                act();
                return true;

            }
            catch (Exception e) {
                AppException(e);
            }
            err();
            return false;
        }
        public static T TryReturn<T>(Func<T> act) where T : class {
            try {
                return act();
            }
            catch (Exception e) {
                AppException(e);
            }
            return null;
        }
        public static T TryReturn<T>(T def, Func<T> act) {
            try {
                return act();
            }
            catch (Exception e) {
                AppException(e);
            }
            return def;
        }
        public static T TryReturn<T>(Func<T> err, Func<T> act) {
            try {
                return act();
            }
            catch (Exception e) {
                AppException(e);
            }
            return err();
        }
        public static bool TryResult(Func<bool> act) {
            try {
                return act();
            }
            catch (Exception e) {
                AppException(e);
            }
            return false;
        }
        public static bool TryResultError(Func<bool> act, Func<bool> err) {
            try {
                return act();
            }
            catch (Exception e) {
                AppException(e);
            }
            return err();
        }
        public static void AppException(Exception e) {
            _appLogger?.Exception(e);
        }
        public static void TryMeasure(Action act) {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            _appLogger.InfoMsg($"Start at {DateTime.Now}");
            TryAction(act);
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            _appLogger.InfoMsg($"Done at {DateTime.Now} [duration {elapsedTime}({watch.ElapsedMilliseconds}ms)]");
        }
        public static void FileNotFound(string fn) => _appLogger.ErrorMsg("Could not find the file " + fn);

        public bool HasError => Errors > 0;
        public long Errors { get; set; } = 0;
        public bool AppError(string err) {
            Errors++;
            _appLogger.ErrorMsg(err);
            return false;
        }

        public void AppWasError() {
            Errors++;
        }
        public void AppFailure(string s) {
            AppError(s);
            throw new InvalidOperationException(s);
        }
    }
}
