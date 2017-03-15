using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ComparisonGroupingFolderContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public ComparisonGroupingFolderContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter)
         : base(classificationNode, presenter)
      {
      }
   }

   public class ComparisonGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      public ComparisonGroupingFolderTreeNodeContextMenuFactory()
         : base(ClassificationType.Comparison)
      {
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ComparisonGroupingFolderContextMenu(classificationNode, presenter);
      }
   }
}