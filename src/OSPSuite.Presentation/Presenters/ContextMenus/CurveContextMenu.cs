using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveContextMenu : ContextMenu<Curve>
   {
      public CurveContextMenu(Curve curve): base(curve)
      {}

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(Curve curve)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit).WithCommandFor<EditCurveUICommand, Curve>(curve);
      }
   }

   public interface ICurveContextMenuFactory : IContextMenuFactory<Curve>
   {}

   public class CurveContextMenuFactory : ICurveContextMenuFactory
   {
      public IContextMenu CreateFor(Curve curve, IPresenterWithContextMenu<Curve> presenter)
      {
         return new CurveContextMenu(curve);
      }
   }
}