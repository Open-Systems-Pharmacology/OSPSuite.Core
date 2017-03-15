using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class AxisContextMenu : ContextMenu<IAxis>
   {
      public AxisContextMenu(IAxis objectRequestingContextMenu) : base(objectRequestingContextMenu)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IAxis axis)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit).WithCommandFor<EditAxisUICommand, IAxis>(axis);
      }
   }

   public interface IAxisContextMenuFactory : IContextMenuFactory<IAxis>
   {

   }

   public class AxisContextMenuFactory : IAxisContextMenuFactory
   {
      public IContextMenu CreateFor(IAxis axis, IPresenterWithContextMenu<IAxis> presenter)
      {
         return new AxisContextMenu(axis);
      }
   }
}
