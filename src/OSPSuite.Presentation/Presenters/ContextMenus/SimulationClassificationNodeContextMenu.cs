using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class SimulationClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public SimulationClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter)
         : base(objectRequestingContextMenu, presenter)
      {
      }
   }

   public class SimulationGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      public SimulationGroupingFolderTreeNodeContextMenuFactory()
         : base(ClassificationType.Simulation)
      {
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new SimulationClassificationNodeContextMenu(classificationNode, presenter);
      }
   }
}