using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class OpenMRUProjectCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;
      public string ProjectPath { get; set; }

      public OpenMRUProjectCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.OpenProjectFrom(ProjectPath);
      }
   }

}