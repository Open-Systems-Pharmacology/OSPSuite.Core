using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Comparisons;

namespace OSPSuite.Presentation.UICommands
{
   public class ComparisonVisibilityUICommand : MainViewItemPresenterVisibilityCommand<IMainComparisonPresenter>
   {
      public ComparisonVisibilityUICommand(IApplicationController applicationController)
         : base(applicationController)
      {
      }
   }
}