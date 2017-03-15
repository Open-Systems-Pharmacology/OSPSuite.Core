using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.UICommands
{
   public class CloseAllButMdiViewCommand : ObjectUICommand<IMdiChildView>
   {
      private readonly IApplicationController _applicationController;

      public CloseAllButMdiViewCommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         var allPresentersToClose = _applicationController.OpenedPresenters().Where(p => p != Subject.Presenter).ToList();
         allPresentersToClose.Each(p => p.Close());
      }
   }
}