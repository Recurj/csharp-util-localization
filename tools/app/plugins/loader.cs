using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace RJToolsApp {
    //    The AssemblyLoadContext type is a special type in the runtime
    //    that allows developers to isolate loaded assemblies into different
    //    groups to ensure that assembly versions don't conflict.
    //    Additionally, a custom AssemblyLoadContext can choose different
    //    paths to load assemblies from and override the default behavior.
    public class AppLoaderPlugin : AssemblyLoadContext {
        // The AssemblyDependencyResolver object is constructed with the path to a.NET class library.
        // It resolves assemblies and native libraries to their relative paths based on the.deps.json file
        // for the class library whose path was passed to the AssemblyDependencyResolver constructor.
        // The custom AssemblyLoadContext enables plugins to have their own dependencies, and
        // the AssemblyDependencyResolver makes it easy to correctly load the dependencies
        private readonly AssemblyDependencyResolver _resolver;
        public AppLoaderPlugin(string pluginPath) {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName) {
            var def=Default.Assemblies
            .FirstOrDefault(x => x.FullName == assemblyName.FullName);
            if (def != null) {
#if DEBUG
                Console.WriteLine("Default loading " + assemblyName.FullName);
#endif
                return def;
            }
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null) {
#if DEBUG
                Console.WriteLine("Custom loading " + assemblyName.FullName);
#endif
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null) {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
        //       public static Assembly GetAssembly(string fn) => Assembly.LoadFrom(fn);
//        public Assembly GetAssembly(string fn) => LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(fn)));
        public Assembly GetAssembly(string fn) =>
            LoadFromAssemblyName(AssemblyName.GetAssemblyName(fn));
        public static T LoadSingle<T>(Assembly assembly) where T : class {
            Type pluginType = typeof(T);
            foreach (var type in assembly.GetTypes()) {
                if (pluginType.IsAssignableFrom(type)) {
                    if (Activator.CreateInstance(type) is T obj) return obj;
                }
            }
            return default;
        }
        public static IEnumerable<T> LoadAll<T>(Assembly assembly) where T : class {
            int count = 0;
            foreach (var type in assembly.GetTypes()) {
                if (typeof(T).IsAssignableFrom(type)) {
                    if (Activator.CreateInstance(type) is T obj) {
                        count++;
                        yield return obj;
                    }
                }
            }
        }
        public static string GetPluginPath(string group) {
            return Path.Combine(
                Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "plugins"), group);
        }
        public static void NotFound(string fn, string plugin) => RJApplication.Logger.ErrorMsg("Could not find the plugin " + plugin + " in the file " + fn);

    }
}
