using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.UICommands
{
   public class EditCurveUICommand : ObjectUICommand<ICurve>
   {
      private readonly IApplicationController _applicationController;

      public EditCurveUICommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var editPresenter = _applicationController.Start<ISingleCurveSettingsModalPresenter>())
         {
            editPresenter.Edit(Subject);
            editPresenter.ShowView();
         }
      }
   }
}