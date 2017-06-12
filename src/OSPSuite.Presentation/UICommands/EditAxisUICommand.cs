using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace OSPSuite.Presentation.UICommands
{
   public class EditAxisUICommand : ObjectUICommand<AxisViewItem>
   {
      private readonly IApplicationController _applicationController;

      public EditAxisUICommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var editPresenter = _applicationController.Start<ISingleAxisSettingsPresenter>())
         {
            editPresenter.Edit(Subject.Chart, Subject.Axis);
         }
      }
   }
}