using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalDiagramBackgroundContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public JournalDiagramBackgroundContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new JournalDiagramBackgroundContextMenu(viewItem.DowncastTo<JournalDiagramBackground>(), presenter.DowncastTo<IJournalDiagramPresenter>(), _container);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<JournalDiagramBackground>() &&
                presenter.IsAnImplementationOf<IJournalDiagramPresenter>();
      }
   }

   public class JournalDiagramBackgroundContextMenu : ContextMenu<JournalDiagramBackground, IJournalDiagramPresenter>
   {
      public JournalDiagramBackgroundContextMenu(JournalDiagramBackground diagramBackground, IJournalDiagramPresenter context, IContainer container)
         : base(diagramBackground, context, container)
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