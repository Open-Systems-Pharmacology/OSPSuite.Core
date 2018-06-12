using Microsoft.Extensions.Logging;

namespace OSPSuite.Core.Services
{
   public class LogEntry
   {
      public LogLevel Level { get; }
      public string Message { get; }

      public LogEntry(LogLevel level, string message)
      {
         Level = level;
         Message = message;
      }

      public override string ToString()
      {
         return Display;
      }

      public string Display => Level == LogLevel.None ? Message : $"{Level}: {Message}";
   }
}