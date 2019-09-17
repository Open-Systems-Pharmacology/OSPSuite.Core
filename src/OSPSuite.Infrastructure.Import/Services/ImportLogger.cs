using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using ILogger = OSPSuite.Core.Services.ILogger;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IImportLogger : ILogger
   {
      IEnumerable<LogEntry> Entries { get; }
      IEnumerable<string> Log { get; }
      string LogString { get; }
      NotificationType Status { get; }
      string FilePath { get; }
   }

   public class ImportLogger : IImportLogger
   {
      public virtual string FilePath { get; set; }

      private readonly List<LogEntry> _entries;

      public ImportLogger()
      {
         _entries = new List<LogEntry>();
      }

      public void AddToLog(string message, LogLevel logLevel, string categoryName)
      {
         _entries.Add(new LogEntry(logLevel, message));
      }

      public virtual IEnumerable<LogEntry> Entries => _entries;

      public virtual IEnumerable<string> Log
      {
         get { return _entries.Select(x => x.ToString()); }
      }

      public virtual string LogString => Log.ToString("\n");

      public virtual NotificationType Status
      {
         get
         {
            var allStatus = _entries.Select(x => x.Level).Distinct().ToList();

            if (allStatus.Contains(LogLevel.Error) || allStatus.Contains(LogLevel.Critical))
               return NotificationType.Error;

            if (allStatus.Contains(LogLevel.Warning))
               return NotificationType.Warning;

            return NotificationType.Info;
         }
      }
   }
}