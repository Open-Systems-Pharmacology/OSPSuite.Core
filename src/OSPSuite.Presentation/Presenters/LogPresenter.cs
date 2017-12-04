using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OSPSuite.Core;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters
{
   public interface ILogPresenter : IPresenter<ILogView>,
      IListener<LogEntryEvent>
   {
      void ClearLog();
      IEnumerable<LogLevel> AvailableLogLevels { get; }
   }

   public class LogPresenter : AbstractPresenter<ILogView, ILogPresenter>, ILogPresenter
   {
      private readonly IStartOptions _startOptions;
      private readonly MessageStatusFilterDTO _messageStatusFilter;

      public LogPresenter(ILogView view, IStartOptions startOptions)
         : base(view)
      {
         _startOptions = startOptions;
         _messageStatusFilter = new MessageStatusFilterDTO();
         _view.BindTo(_messageStatusFilter);
      }

      public void Handle(LogEntryEvent eventToHandle)
      {
         var logEntry = eventToHandle.LogEntry;
         if (shouldAddMessageToLog(logEntry))
            _view.AddLog(logEntry.Display);
      }

      public void ClearLog()
      {
         _view.ClearLog();
      }

      public IEnumerable<LogLevel> AvailableLogLevels
      {
         get
         {
            if (_startOptions.IsDeveloperMode)
               yield return LogLevel.Debug;

            yield return LogLevel.Information;
            yield return LogLevel.Warning;
            yield return LogLevel.Error;
         }
      }

      private bool shouldAddMessageToLog(LogEntry logEntry)
      {
         return logEntry.Level == LogLevel.None || logEntry.Level.Is(_messageStatusFilter.LogLevel);
      }
   }
}