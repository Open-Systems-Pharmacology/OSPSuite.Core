using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class UpdateAllDisplayUnitsUICommand : IUICommand
   {
      private readonly IDisplayUnitUpdater _displayUnitUpdater;
      private readonly IProjectRetriever _projectRetriever;

      public UpdateAllDisplayUnitsUICommand(IDisplayUnitUpdater displayUnitUpdater, IProjectRetriever projectRetriever)
      {
         _displayUnitUpdater = displayUnitUpdater;
         _projectRetriever = projectRetriever;
      }

      public void Execute()
      {
         _displayUnitUpdater.UpdateDisplayUnitsIn(_projectRetriever.CurrentProject);
      }
   }
}