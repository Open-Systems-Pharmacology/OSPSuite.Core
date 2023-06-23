using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class SimulationClassificationNodeContextMenu : ExplorerClassificationNodeContextMenu
   {
      public SimulationClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter, IContainer container)
         : base(objectRequestingContextMenu, presenter, container)
      {
      }
   }

   public class SimulationGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public SimulationGroupingFolderTreeNodeContextMenuFactory(IContainer container)
         : base(ClassificationType.Simulation)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new SimulationClassificationNodeContextMenu(classificationNode, presenter, _container);
      }
   }
}