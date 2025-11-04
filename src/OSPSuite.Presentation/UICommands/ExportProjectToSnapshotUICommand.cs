using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ExportProjectToSnapshotUICommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public ExportProjectToSnapshotUICommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public async void Execute() => await _projectTask.SecureAwait(x => x.ExportCurrentProjectToSnapshot());
   }
}