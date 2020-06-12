using Microsoft.Extensions.Logging;
using System;

namespace OSPSuite.Core.Services
{
   public interface ILogger
   {
      /// <summary>
      ///    Logs the <paramref name="message" /> using the provided <paramref name="logLevel" /> for the
      ///    <paramref name="categoryName" />
      /// </summary>
      void AddToLog(string message, LogLevel logLevel, string categoryName);

      /// <summary>
      /// Adds a new configuration to the loggingBuilder
      /// </summary>
      /// <param name="configuration">New configuration</param>
      void AddLoggingBuilderConfiguration(Func<ILoggingBuilder, ILoggingBuilder> configuration);

      /// <summary>
      /// Adds a loggerProvider
      /// </summary>
      /// <param name="loggerprovider"></param>
      void AddLoggerProvider(ILoggerProvider loggerprovider);
   }
}