using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class ClearHistoryUICommand : IUICommand
   {
      private readonly IHistoryTask _historyTask;

      public ClearHistoryUICommand(IHistoryTask historyTask)
      {
         _historyTask = historyTask;
      }

      public void Execute()
      {
         _historyTask.ClearHistory();
      }
   }
}