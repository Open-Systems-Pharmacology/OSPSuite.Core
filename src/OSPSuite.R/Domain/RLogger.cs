using System;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;
using IOSPLogger = OSPSuite.Core.Services.IOSPLogger;

namespace OSPSuite.R.Domain
{
   public class RLogger : IOSPLogger
   {
      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Console.WriteLine($"{logLevel} - {message}");
      }

      public void Log(IImportLogger importLogger)
      {
         if (importLogger.Status.Is(NotificationType.Warning))
            importLogger.Log.Each(x => this.AddWarning(x));
      }
   }
}