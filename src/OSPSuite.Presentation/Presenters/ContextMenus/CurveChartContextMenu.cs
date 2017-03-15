using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class CurveChartContextMenu : ContextMenu<ICurveChart, IChartDisplayPresenter>
   {
      public CurveChartContextMenu(ICurveChart objectRequestingContextMenu, IChartDisplayPresenter context)
         : base(objectRequestingContextMenu, context)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ICurveChart curveChart, IChartDisplayPresenter chartDisplayPresenter)
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

   public interface ICurveChartContextMenuFactory : IContextMenuFactory<ICurveChart>
   {
   }

   public class CurveChartContextMenuFactory : ICurveChartContextMenuFactory
   {
      public IContextMenu CreateFor(ICurveChart objectRequestingContextMenu, IPresenterWithContextMenu<ICurveChart> presenter)
      {
         return new CurveChartContextMenu(objectRequestingContextMenu, presenter.DowncastTo<IChartDisplayPresenter>());
      }
   }
}