using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveViewItem : IViewItem
   {
      public IChart Chart { get; }
      public Curve Curve { get; }

      public CurveViewItem(IChart chart, Curve curve)
      {
         Chart = chart;
         Curve = curve;
      }
   }

   public class CurveContextMenu : ContextMenu<CurveViewItem>
   {
      public CurveContextMenu(CurveViewItem curveViewItem) : base(curveViewItem)
      {
         
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(CurveViewItem curveViewItem)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditCurveUICommand, CurveViewItem>(curveViewItem);
      }
   }

   public class CurveContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new CurveContextMenu(viewItem.DowncastTo<CurveViewItem>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<CurveViewItem>();
      }
   }
}