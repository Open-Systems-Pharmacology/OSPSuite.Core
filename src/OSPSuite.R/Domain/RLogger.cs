using System;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public class RLogger : IOSPSuiteLogger
   {
      public string DefaultCategoryName { get; set; }

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