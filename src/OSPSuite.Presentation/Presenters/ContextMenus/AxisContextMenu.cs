using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class AxisViewItem : IViewItem
   {
      public IChart Chart { get; }
      public Axis Axis { get; }

      public AxisViewItem(IChart chart, Axis axis)
      {
         Chart = chart;
         Axis = axis;
      }
   }

   public class AxisContextMenu : ContextMenu<AxisViewItem>
   {
      public AxisContextMenu(AxisViewItem axisViewItem) : base(axisViewItem)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(AxisViewItem axisViewItem)
      {
         yield return CreateMenuButton.WithCaption(Captions.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditAxisUICommand, AxisViewItem>(axisViewItem);
      }
   }

   public class AxisContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new AxisContextMenu(viewItem.DowncastTo<AxisViewItem>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<AxisViewItem>();
      }
   }
}