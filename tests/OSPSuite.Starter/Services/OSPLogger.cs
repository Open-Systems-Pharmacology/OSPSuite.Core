using System.Diagnostics;
using Microsoft.Extensions.Logging;
using IOSPLogger = OSPSuite.Core.Services.IOSPLogger;

namespace OSPSuite.Starter.Services
{
   public class OSPLogger : IOSPLogger
   {
      public void AddToLog(string message, LogLevel logLevel)
      {
      }

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         Debug.Print($"{logLevel} - {message} - {categoryName}");
      }
   }
}