
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace WebStore.Logger
{
    public static class Log4NetLoggerFactory
    {
        public static string CheckFilePath(string path)
        {
            if (path is not { Length: > 0 })
                throw new ArgumentException("The path to the config file wasn't specified");
            if (Path.IsPathRooted(path))
                return path;

            var assembly = Assembly.GetEntryAssembly();
            var dir = Path.GetDirectoryName(assembly!.Location);

            return Path.Combine(dir!, path);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory Factory, string ConfigurationFile = "log4net.config")
        {
            Factory.AddProvider(new Log4NetLoggerProvider(ConfigurationFile));

            return Factory;
        }
    }
}
