using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ObjectBaseCommonContextMenuItems
   {
      public static IMenuBarItem AddToJournal(IObjectBase objectBase)
      {
         return AddToJournal(new[] {objectBase});
      }

      public static IMenuBarItem AddToJournal(IReadOnlyList<IObjectBase> objects)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddRelatedItemsToActiveJournalPageUICommand, IReadOnlyList<IObjectBase>>(objects)
            .WithIcon(ApplicationIcons.AddToJournal);
      }
   }
}