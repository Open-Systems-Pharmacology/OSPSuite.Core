using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalPageNodeContextMenuFactory : IContextMenuSpecificationFactory<IReadOnlyList<IBaseNode>>
   {
      public IContextMenu CreateFor(IReadOnlyList<IBaseNode> nodes, IPresenterWithContextMenu<IReadOnlyList<IBaseNode>> presenter)
      {
         return new JournalPageNodeContextMenu(nodes.Cast<IJournalPageNode>().ToList(), presenter.DowncastTo<IJournalDiagramPresenter>());
      }

      public bool IsSatisfiedBy(IReadOnlyList<IBaseNode> baseNodes, IPresenterWithContextMenu<IReadOnlyList<IBaseNode>> presenter)
      {
         return baseNodes.All(baseNode => baseNode.IsAnImplementationOf<IJournalPageNode>())
                && presenter.IsAnImplementationOf<IJournalDiagramPresenter>();
      }
   }

   public class JournalPageNodeContextMenu : ContextMenu<IReadOnlyList<IJournalPageNode>, IJournalDiagramPresenter>
   {
      public JournalPageNodeContextMenu(IReadOnlyList<IJournalPageNode> baseNode, IJournalDiagramPresenter presenter)
         : base(baseNode, presenter)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IJournalPageNode> journalPageNode, IJournalDiagramPresenter presenter)
      {
         if (journalPageNode.Count == 1)
            yield return CreateMenuButton.WithCaption(MenuNames.Edit)
               .WithActionCommand(() => presenter.EditJournalPage(journalPageNode[0]))
               .WithIcon(ApplicationIcons.PageEdit);

         yield return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithActionCommand(presenter.DeleteSelection)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }
}
