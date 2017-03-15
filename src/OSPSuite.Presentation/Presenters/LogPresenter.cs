using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface ILogPresenter : IPresenter<ILogView>,
      IListener<LogEntryEvent>
   {
      void ClearLog();
   }

   public class LogPresenter : AbstractPresenter<ILogView, ILogPresenter>, ILogPresenter
   {
      private readonly MessageStatusFilterDTO _messageStatusFilter;

      public LogPresenter(ILogView view)
         : base(view)
      {
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

      private bool shouldAddMessageToLog(LogEntry logEntry)
      {
         return logEntry.MessageStatus == NotificationType.None || (logEntry.MessageStatus.Is(_messageStatusFilter.Status));
      }
   }
}