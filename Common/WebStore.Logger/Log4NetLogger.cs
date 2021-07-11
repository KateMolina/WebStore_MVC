
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace WebStore.Logger
{

    public class Log4NetLogger : ILogger
    {
        private readonly ILog iLog;

        public Log4NetLogger(string Category, XmlElement xmlElement)
        {
            var logger_repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            iLog = LogManager.GetLogger(logger_repository.Name, Category);
            log4net.Config.XmlConfigurator.Configure(logger_repository, xmlElement);
        }
        public IDisposable BeginScope<TState>(TState state) => null;


        public bool IsEnabled(LogLevel logLevel) => logLevel switch
        {
            LogLevel.None => false,
            LogLevel.Trace => iLog.IsDebugEnabled,
            LogLevel.Debug => iLog.IsDebugEnabled,
            LogLevel.Information => iLog.IsInfoEnabled,
            LogLevel.Warning => iLog.IsWarnEnabled,
            LogLevel.Error => iLog.IsErrorEnabled,
            LogLevel.Critical => iLog.IsFatalEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(LogLevel), logLevel, null)
        };
        

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
         
            if(formatter is null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            if (!IsEnabled(logLevel)) return;
            var log_message = formatter(state, exception);

            if (string.IsNullOrEmpty(log_message) && exception is null)
                return;

            switch (logLevel)
            {

                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                case LogLevel.None: break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    iLog.Debug(log_message); break;
                case LogLevel.Information:
                    iLog.Info(log_message); break;
                case LogLevel.Warning:
                    iLog.Warn(log_message); break;
                case LogLevel.Error:
                    iLog.Error(log_message, exception); break;
                case LogLevel.Critical:
                    iLog.Fatal(log_message, exception); break;
            }

        }
    }
}
