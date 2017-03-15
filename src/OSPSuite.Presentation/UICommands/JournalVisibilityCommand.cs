using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.UICommands
{
   public class JournalVisibilityCommand : MainViewItemPresenterVisibilityCommand<IJournalPresenter>
   {
      public JournalVisibilityCommand(IApplicationController applicationController)
         : base(applicationController)
      {
      }
   }
}