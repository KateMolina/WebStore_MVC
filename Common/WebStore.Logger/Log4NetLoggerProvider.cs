
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string configurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

        public Log4NetLoggerProvider(string ConfigurationFile)
        {
            configurationFile = ConfigurationFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
             {
                 var xml = new XmlDocument();
                 xml.Load(configurationFile);
                 return new Log4NetLogger(category, xml["log4net"]);

             });
        }

        public void Dispose()
        {
            _Loggers.Clear();
        }
    }
}
