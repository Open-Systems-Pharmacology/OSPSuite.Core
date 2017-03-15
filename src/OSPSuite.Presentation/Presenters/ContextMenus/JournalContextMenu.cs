using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new JournalContextMenu(viewItem.DowncastTo<JournalDTO>(), presenter.DowncastTo<IJournalPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<JournalDTO>()
                && presenter.IsAnImplementationOf<IJournalPresenter>();
      }
   }

   public class JournalContextMenu : ContextMenu<JournalDTO,IJournalPresenter>
   {
      public JournalContextMenu(JournalDTO journalDTO, IJournalPresenter journalPresenter)
         : base(journalDTO, journalPresenter)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(JournalDTO journalDTO, IJournalPresenter journalPresenter)
      {
         yield return CreateMenuButton.WithCaption(Captions.Journal.CreateJournalPageMenu)
            .WithIcon(ApplicationIcons.PageAdd)
            .WithCommand<CreateJournalPageUICommand>();
      }
   }
}