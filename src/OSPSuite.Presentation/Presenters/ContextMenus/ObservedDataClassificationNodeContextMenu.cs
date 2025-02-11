using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class ObservedDataClassificationNodeContextMenu : ExplorerClassificationNodeContextMenu
   {
      public ObservedDataClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, IExplorerPresenter presenter, IContainer container)
         : base(objectRequestingContextMenu, presenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllCustomMenuItemsFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new[] {ObservedDataClassificationCommonContextMenuItems.EditMultipleMetaData(classificationNode, _container)};
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