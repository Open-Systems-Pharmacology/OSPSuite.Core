using System.Data.Common;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.ORM.MetaData;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure.Services
{
   public class HistoryTask: IHistoryTask
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly IHistoryManagerRetriever _historyManagerRetriever;
      private readonly IDialogCreator _dialogCreator;
      private readonly SQLiteProjectCommandExecuter _commandExecuter;
      private readonly IEventPublisher _eventPublisher;
      private readonly ICommandMetaDataRepository _commandMetaDataRepository;

      public HistoryTask(
         IProjectRetriever projectRetriever, 
         IHistoryManagerRetriever historyManagerRetriever, 
         IDialogCreator dialogCreator, 
         SQLiteProjectCommandExecuter commandExecuter,
         IEventPublisher eventPublisher,
         ICommandMetaDataRepository commandMetaDataRepository
         )

      {
         _projectRetriever = projectRetriever;
         _historyManagerRetriever = historyManagerRetriever;
         _dialogCreator = dialogCreator;
         _commandExecuter = commandExecuter;
         _eventPublisher = eventPublisher;
         _commandMetaDataRepository = commandMetaDataRepository;
      }

      public void ClearHistory()
      {
         var clear = _dialogCreator.MessageBoxYesNo(Captions.ReallyClearHistory, Captions.Clear, Captions.CancelButton);
         if (clear == ViewResult.No) 
            return;

         clearProjectHistory();
         clearCurrentHistory();
      }

      private void clearCurrentHistory()
      {
         var historyManager = _historyManagerRetriever.Current;
         if (historyManager == null)
            return;

         historyManager.Clear();
         _eventPublisher.PublishEvent(new HistoryClearedEvent());
      }

      private void clearProjectHistory()
      {
         var projectPath = _projectRetriever.ProjectFullPath;
         if (string.IsNullOrEmpty(projectPath))
            return;

         _commandExecuter.ExecuteCommand(projectPath, clearCommand);
         _commandMetaDataRepository.Clear();
      }

      private void clearCommand(DbConnection connection)
      {
         connection.ExecuteNonQuery("DELETE FROM HISTORY_ITEMS;");
         connection.ExecuteNonQuery("DELETE FROM COMMAND_PROPERTIES;");
         connection.ExecuteNonQuery("DELETE FROM COMMANDS;");
      }
   }
}