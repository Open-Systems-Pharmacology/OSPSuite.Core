using OSPSuite.Core.Services;

namespace OSPSuite.Core.Events
{
   public class LogEntryEvent
   {
      public LogEntry LogEntry { get; private set; }

      public LogEntryEvent(LogEntry logEntry)
      {
         LogEntry = logEntry;
      }
   }
}