using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ModuleClassificationNodeContextMenu : ExplorerClassificationNodeContextMenu
   {
      public ModuleClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter, IContainer container)
         : base(objectRequestingContextMenu, presenter, container)
      {
      }
   }

   public class ModuleGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public ModuleGroupingFolderTreeNodeContextMenuFactory(IContainer container)
         : base(ClassificationType.Module)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ModuleClassificationNodeContextMenu(classificationNode, presenter, _container);
      }
   }
}