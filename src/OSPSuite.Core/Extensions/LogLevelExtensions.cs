using Microsoft.Extensions.Logging;

namespace OSPSuite.Core.Extensions
{
   public static class LogLevelExtensions
   {
      public static bool Is(this LogLevel logLevel, LogLevel otherLogLevel)
      {
         return logLevel >= otherLogLevel;
      }
   }
}