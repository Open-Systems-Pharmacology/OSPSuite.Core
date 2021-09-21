using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Services
{
   public class OSPSuiteLogger : IOSPSuiteLogger
   {
      public string DefaultCategoryName { get; set; }

      protected const string DEFAULT_LOGGER_CATEGORY = "OSPSuite";
      private readonly ILoggerCreator _loggerCreator;

      public OSPSuiteLogger(ILoggerCreator loggerCreator, string defaultCategoryName = DEFAULT_LOGGER_CATEGORY)
      {
         _loggerCreator = loggerCreator;
         DefaultCategoryName = defaultCategoryName;
      }

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         var logger = _loggerCreator.GetOrCreateLogger(string.IsNullOrEmpty(categoryName) ? DEFAULT_LOGGER_CATEGORY : categoryName);
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
