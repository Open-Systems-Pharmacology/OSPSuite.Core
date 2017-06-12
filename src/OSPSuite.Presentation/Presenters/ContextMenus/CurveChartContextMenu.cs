using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveChartContextMenu : ContextMenu<CurveChart, IChartDisplayPresenter>
   {
      public CurveChartContextMenu(CurveChart objectRequestingContextMenu, IChartDisplayPresenter context)
         : base(objectRequestingContextMenu, context)
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

   public interface ICurveChartContextMenuFactory : IContextMenuFactory<CurveChart>
   {
   }

   public class CurveChartContextMenuFactory : ICurveChartContextMenuFactory
   {
      public IContextMenu CreateFor(CurveChart objectRequestingContextMenu, IPresenterWithContextMenu<CurveChart> presenter)
      {
         return new CurveChartContextMenu(objectRequestingContextMenu, presenter.DowncastTo<IChartDisplayPresenter>());
      }
   }
}