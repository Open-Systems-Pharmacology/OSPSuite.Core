using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalPageContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public JournalPageContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new JournalPageContextMenu(viewItem.DowncastTo<JournalPageDTO>(), presenter.DowncastTo<IJournalPresenter>(), _container);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<JournalPageDTO>()
                && presenter.IsAnImplementationOf<IJournalPresenter>();
      }
   }

   public class JournalPageContextMenu : ContextMenu<JournalPageDTO, IJournalPresenter>
   {
      public JournalPageContextMenu(JournalPageDTO journalPageDTO, IJournalPresenter presenter, IContainer container)
         : base(journalPageDTO, presenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(JournalPageDTO journalPageDTO, IJournalPresenter presenter)
      {
         yield return CreateMenuButton.WithCaption(MenuNames.Edit)
            .WithActionCommand(() => presenter.Edit(journalPageDTO))
            .WithIcon(ApplicationIcons.PageEdit);

         yield return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithActionCommand(() => presenter.Delete(journalPageDTO))
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();

      }
   }
}