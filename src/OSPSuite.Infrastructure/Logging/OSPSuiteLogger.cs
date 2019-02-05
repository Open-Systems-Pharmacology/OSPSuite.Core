using Microsoft.Extensions.Logging;
using ILogger = OSPSuite.Core.Services.ILogger;

namespace OSPSuite.Infrastructure.Logging
{
   public class OSPSuiteLogger : ILogger
   {
      private readonly ILoggerFactory _loggerFactory;
      private readonly string _defaultLoggerCategory;

      public OSPSuiteLogger(ILoggerFactory loggerFactory, string defaultLoggerCategory = "OSPSuite")
      {
         _loggerFactory = loggerFactory;
         _defaultLoggerCategory = defaultLoggerCategory;
      }

      public virtual void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         var logger = _loggerFactory.CreateLogger(string.IsNullOrEmpty(categoryName) ? _defaultLoggerCategory : categoryName);
         switch (logLevel)
         {
            case LogLevel.Trace:
               logger.LogTrace(message);
               break;
            case LogLevel.Debug:
               logger.LogDebug(message);
               break;
            case LogLevel.Information:
               logger.LogInformation(message);
               break;
            case LogLevel.Warning:
               logger.LogWarning(message);
               break;
            case LogLevel.Error:
               logger.LogError(message);
               break;
            case LogLevel.Critical:
               logger.LogCritical(message);
               break;
         }
      }
   }
}