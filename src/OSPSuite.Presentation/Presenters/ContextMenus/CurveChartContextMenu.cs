using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveChartViewItem : IViewItem
   {
      public CurveChart Chart { get; }

      public CurveChartViewItem(CurveChart chart)
      {
         Chart = chart;
      }
   }

   public class CurveChartContextMenu : ContextMenu<CurveChart, IChartDisplayPresenter>
   {
      public CurveChartContextMenu(CurveChart curveChart, IChartDisplayPresenter context)
         : base(curveChart, context)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(CurveChart curveChart, IChartDisplayPresenter chartDisplayPresenter)
      {
         yield return CreateMenuButton.WithCaption(MenuNames.ResetZoom)
            .WithActionCommand(chartDisplayPresenter.ResetZoom)
            .WithIcon(ApplicationIcons.Reset);

         yield return CreateMenuButton.WithCaption(MenuNames.CopyToClipboard)
            .WithActionCommand(chartDisplayPresenter.CopyToClipboard)
            .WithIcon(ApplicationIcons.Copy)
            .AsGroupStarter();

         yield return CreateMenuButton.WithCaption(MenuNames.ExportToPDF)
            .WithActionCommand(chartDisplayPresenter.ExportToPDF)
            .WithIcon(ApplicationIcons.PDF);

         yield return CreateMenuButton.WithCaption(MenuNames.ExportToExcel)
            .WithActionCommand(chartDisplayPresenter.ExportToExcel)
            .WithIcon(ApplicationIcons.Excel);
      }
   }

   public class CurveChartContextMenuFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new CurveChartContextMenu(viewItem.DowncastTo<CurveChartViewItem>().Chart, presenter.DowncastTo<IChartDisplayPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<CurveChartViewItem>()
                && presenter.IsAnImplementationOf<IChartDisplayPresenter>();
      }
   }
}