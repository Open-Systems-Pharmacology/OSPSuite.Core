using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveContextMenu : ContextMenu<ICurve>
   {
      public CurveContextMenu(ICurve curve): base(curve)
      {}

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ICurve curve)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit).WithCommandFor<EditCurveUICommand, ICurve>(curve);
      }
   }

   public interface ICurveContextMenuFactory : IContextMenuFactory<ICurve>
   {}

   public class CurveContextMenuFactory : ICurveContextMenuFactory
   {
      public IContextMenu CreateFor(ICurve curve, IPresenterWithContextMenu<ICurve> presenter)
      {
         return new CurveContextMenu(curve);
      }
   }
}