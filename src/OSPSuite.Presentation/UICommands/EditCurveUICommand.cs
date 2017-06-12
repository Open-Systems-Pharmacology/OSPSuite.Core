using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace OSPSuite.Presentation.UICommands
{
   public class EditCurveUICommand : ObjectUICommand<CurveViewItem>
   {
      private readonly IApplicationController _applicationController;

      public EditCurveUICommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var editPresenter = _applicationController.Start<ISingleCurveSettingsPresenter>())
         {
            editPresenter.Edit(Subject.Chart, Subject.Curve);
         }
      }
   }
}