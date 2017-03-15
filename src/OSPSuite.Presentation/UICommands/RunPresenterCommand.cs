using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.UICommands
{
   public class RunPresenterCommand<TPresenter> : IUICommand where TPresenter : class, IDisposablePresenter
   {
      private readonly IApplicationController _applicationController;

      public RunPresenterCommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public virtual void Execute()
      {
         using (_applicationController.Start<TPresenter>())
         {
         }
      }
   }
}