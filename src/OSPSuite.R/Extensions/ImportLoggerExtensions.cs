using System;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Extensions
{
   public static class ImportLoggerExtensions
   {
      public static void LogToR(this IImportLogger importLogger)
      {
         if (importLogger.Status.Is(NotificationType.Error))
            throw new OSPSuiteException(importLogger.LogString);

         if (importLogger.Status.Is(NotificationType.Warning))
            importLogger.Log.Each(Console.WriteLine);
      }
   }
}