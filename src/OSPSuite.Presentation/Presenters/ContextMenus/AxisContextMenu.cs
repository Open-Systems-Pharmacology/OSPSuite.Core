using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class AxisContextMenu : ContextMenu<Axis>
   {
      public AxisContextMenu(Axis objectRequestingContextMenu) : base(objectRequestingContextMenu)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(Axis axis)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit).WithCommandFor<EditAxisUICommand, Axis>(axis);
      }
   }

   public interface IAxisContextMenuFactory : IContextMenuFactory<Axis>
   {

   }

   public class AxisContextMenuFactory : IAxisContextMenuFactory
   {
      public IContextMenu CreateFor(Axis axis, IPresenterWithContextMenu<Axis> presenter)
      {
         return new AxisContextMenu(axis);
      }
   }
}
