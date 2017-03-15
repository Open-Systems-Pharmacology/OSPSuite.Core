using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Starter.Presenters
{
   public class MenuBarItemRepository : OSPSuite.Presentation.Repositories.MenuBarItemRepository
   {
      public MenuBarItemRepository(IContainer container) : base(container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuBarItems()
      {
         return Enumerable.Empty<IMenuBarItem>();
      }
   }
}