using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.UICommands
{
   public class EditAxisUICommand : ObjectUICommand<IAxis>
   {
      private readonly IApplicationController _applicationController;

      public EditAxisUICommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var editPresenter = _applicationController.Start<ISingleAxisSettingsModalPresenter>())
         {
            editPresenter.Edit(Subject);
            editPresenter.ShowView();
         }
      }
   }
}
