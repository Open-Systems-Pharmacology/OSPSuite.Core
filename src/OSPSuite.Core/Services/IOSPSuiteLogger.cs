using Microsoft.Extensions.Logging;

namespace OSPSuite.Core.Services
{
   public interface IOSPSuiteLogger
   {
      string DefaultCategoryName { get; set; }

      /// <summary>
      ///    Logs the <paramref name="message" /> using the provided <paramref name="logLevel" /> for the
      ///    <paramref name="categoryName" />
      /// </summary>
      void AddToLog(string message, LogLevel logLevel, string categoryName);
   }
}