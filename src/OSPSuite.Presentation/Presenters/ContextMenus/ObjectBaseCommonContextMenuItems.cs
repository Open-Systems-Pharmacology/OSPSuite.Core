using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ObjectBaseCommonContextMenuItems
   {
      public static IMenuBarItem AddToJournal(IObjectBase objectBase, IContainer container)
      {
         return AddToJournal(new[] { objectBase }, container);
      }

      public static IMenuBarItem AddToJournal(IReadOnlyList<IObjectBase> objects, IContainer container)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddRelatedItemsToActiveJournalPageUICommand, IReadOnlyList<IObjectBase>>(objects, container)
            .WithIcon(ApplicationIcons.AddToJournal);
      }
   }
}