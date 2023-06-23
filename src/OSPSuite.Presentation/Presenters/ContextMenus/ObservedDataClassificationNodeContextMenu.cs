using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ObservedDataClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public ObservedDataClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter, IContainer container)
         : base(objectRequestingContextMenu, presenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         yield return ObservedDataClassificationCommonContextMenuItems.EditMultipleMetaData(classificationNode, _container);

         foreach (var item in base.AllMenuItemsFor(classificationNode, presenter))
         {
            yield return item;
         }
      }
   }

   public class ObservedDataGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public ObservedDataGroupingFolderTreeNodeContextMenuFactory(IContainer container)
         : base(ClassificationType.ObservedData)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new ObservedDataClassificationNodeContextMenu(classificationNode, presenter, _container);
      }
   }
}