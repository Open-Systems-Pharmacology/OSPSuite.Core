using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ComparisonGroupingFolderContextMenu : ExplorerClassificationNodeContextMenu
   {
      public ComparisonGroupingFolderContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter, IContainer container)
         : base(classificationNode, presenter, container)
      {
      }
   }

   public class ComparisonGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public ComparisonGroupingFolderTreeNodeContextMenuFactory(IContainer container)
         : base(ClassificationType.Comparison)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ComparisonGroupingFolderContextMenu(classificationNode, presenter, _container);
      }
   }
}