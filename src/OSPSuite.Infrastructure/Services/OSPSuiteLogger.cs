using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Services
{
   public class OSPSuiteLogger : IOSPSuiteLogger
   {
      protected const string DEFAULT_LOGGER_CATEGORY = "OSPSuite";

      public string DefaultCategoryName { get; set; } = DEFAULT_LOGGER_CATEGORY;

      private readonly ILoggerCreator _loggerCreator;

      public OSPSuiteLogger(ILoggerCreator loggerCreator)
      {
         _loggerCreator = loggerCreator;
      }

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         var logger = _loggerCreator.GetOrCreateLogger(string.IsNullOrEmpty(categoryName) ? DefaultCategoryName : categoryName);
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