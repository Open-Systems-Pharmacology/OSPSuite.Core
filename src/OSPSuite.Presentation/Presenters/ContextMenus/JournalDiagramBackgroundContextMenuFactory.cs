using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalDiagramBackgroundContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new JournalDiagramBackgroundContextMenu(viewItem.DowncastTo<JournalDiagramBackground>(), presenter.DowncastTo<IJournalDiagramPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<JournalDiagramBackground>() &&
                presenter.IsAnImplementationOf<IJournalDiagramPresenter>();
      }
   }

   public class JournalDiagramBackgroundContextMenu : ContextMenu<JournalDiagramBackground, IJournalDiagramPresenter>
   {
      public JournalDiagramBackgroundContextMenu(JournalDiagramBackground diagramBackground, IJournalDiagramPresenter context)
         : base(diagramBackground, context)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(JournalDiagramBackground diagramBackground, IJournalDiagramPresenter presenter)
      {
         yield return CreateMenuButton.WithCaption(MenuNames.CopyToClipboard)
            .WithActionCommand(presenter.CopyDiagramToClipboard)
            .WithIcon(ApplicationIcons.Copy);

         yield return CreateMenuButton.WithCaption(MenuNames.ResetZoom)
            .WithActionCommand(() => presenter.Zoom(0))
            .WithIcon(ApplicationIcons.Reset);

         yield return CreateMenuButton.WithCaption(MenuNames.HideRelatedItems)
            .WithActionCommand(presenter.HideRelatedItems);

         yield return CreateMenuButton.WithCaption(MenuNames.ShowRelatedItems)
            .WithActionCommand(presenter.ShowRelatedItems);
      }
   }
}