using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = OSPSuite.Core.Services.ILogger;

namespace OSPSuite.Starter.Services
{
   public class OSPLogger : ILogger
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