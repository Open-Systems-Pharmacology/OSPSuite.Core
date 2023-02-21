using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public JournalContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new JournalContextMenu(viewItem.DowncastTo<JournalDTO>(), presenter.DowncastTo<IJournalPresenter>(), _container);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<JournalDTO>()
                && presenter.IsAnImplementationOf<IJournalPresenter>();
      }
   }

   public class JournalContextMenu : ContextMenu<JournalDTO, IJournalPresenter>
   {
      public JournalContextMenu(JournalDTO journalDTO, IJournalPresenter journalPresenter, IContainer container)
         : base(journalDTO, journalPresenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(JournalDTO journalDTO, IJournalPresenter journalPresenter)
      {
         yield return CreateMenuButton.WithCaption(Captions.Journal.CreateJournalPageMenu)
            .WithIcon(ApplicationIcons.PageAdd)
            .WithCommand<CreateJournalPageUICommand>(_container);
      }
   }
}