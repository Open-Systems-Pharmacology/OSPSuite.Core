using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class LoadProjectFromSnapshotUICommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public LoadProjectFromSnapshotUICommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.LoadProjectFromSnapshot();
      }
   }
}