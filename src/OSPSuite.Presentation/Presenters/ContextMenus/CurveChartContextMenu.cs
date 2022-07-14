using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
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

         yield return CreateMenuButton.WithCaption(MenuNames.ExportToExcel)
            .WithActionCommand(chartDisplayPresenter.ExportToExcel)
            .WithIcon(ApplicationIcons.Excel);

         if (curveChart.IsAnImplementationOf<ParameterIdentificationPredictedVsObservedChart>() ||
             curveChart.IsAnImplementationOf<SimulationPredictedVsObservedChart>())
         {
            yield return CreateMenuButton.WithCaption(MenuNames.AddDeviationLines)
               .WithActionCommand(chartDisplayPresenter.AddDeviationLines); //and an action I guess....but then we have to publish???
            //.WithIcon(ApplicationIcons.PKSim); //we probably need a new icon here
         }
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