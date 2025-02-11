using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public abstract class ExplorerClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      protected ExplorerClassificationNodeContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter, IContainer container) : base(classificationNode, presenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllCustomMenuItemsFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return presenter.AllCustomMenuItemsFor(classificationNode);
      }
   }
}