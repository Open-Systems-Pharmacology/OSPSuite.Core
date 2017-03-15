using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class CloseAllMdiViewCommand : IUICommand
   {
      private readonly IApplicationController _applicationController;

      public CloseAllMdiViewCommand(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void Execute()
      {
         _applicationController.CloseAll();
      }
   }
}