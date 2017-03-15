using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Main;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class MainViewItemPresenterVisibilityCommand<TPresenter> : IUICommand where TPresenter : IMainViewItemPresenter
   {
      private readonly IApplicationController _applicationController;

      protected MainViewItemPresenterVisibilityCommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void Execute()
      {
         var presenter = _applicationController.Start<TPresenter>();
         presenter.ToggleVisibility();
      }
   }
}