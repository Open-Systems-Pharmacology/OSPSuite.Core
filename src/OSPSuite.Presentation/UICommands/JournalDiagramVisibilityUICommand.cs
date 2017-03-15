using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.UICommands
{
   public class JournalDiagramVisibilityUICommand : MainViewItemPresenterVisibilityCommand<IJournalDiagramMainPresenter>
   {
      public JournalDiagramVisibilityUICommand(IApplicationController applicationController)
         : base(applicationController)
      {
      }
   }
}