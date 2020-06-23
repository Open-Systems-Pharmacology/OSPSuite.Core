using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Services;

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