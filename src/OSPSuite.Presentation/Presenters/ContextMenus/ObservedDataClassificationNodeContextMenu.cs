using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ObservedDataClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public ObservedDataClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter)
         : base(objectRequestingContextMenu, presenter)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         yield return ObservedDataClassificationCommonContextMenuItems.CreateEditMultipleMetaDataMenuButton(classificationNode);

         foreach (var item in base.AllMenuItemsFor(classificationNode, presenter))
         {
            yield return item;
         }
      }
   }

   public class ObservedDataGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      public ObservedDataGroupingFolderTreeNodeContextMenuFactory()
         : base(ClassificationType.ObservedData)
      {
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ObservedDataClassificationNodeContextMenu(classificationNode, presenter);
      }
   }
}