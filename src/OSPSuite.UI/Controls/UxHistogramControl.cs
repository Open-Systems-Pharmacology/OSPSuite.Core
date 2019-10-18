using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Formatters;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Controls
{
   public class UxHistogramControl : UxChartControl
   {
      private ChartTitle _title;
      private IToolTipCreator _toolTipCreator;
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();
      private readonly IntFormatter _intFormatter = new IntFormatter();

      [Browsable(false)]
      public Func<string, Color> StartColorFor { get; set; } = s => Colors.Blue1;

      [Browsable(false)]
      public Func<string, Color> EndColorFor { get; set; } = s => Colors.Blue2;

      public void Initialize(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         _title = new ChartTitle
         {
            Text = string.Empty,
            Font = new Font("Arial", 16),
            Alignment = StringAlignment.Center,
            Dock = ChartTitleDockStyle.Top
         };
         Titles.Add(_title);
         Images = imageListRetriever.AllImages16x16;
         CrosshairEnabled = DefaultBoolean.False;
         SelectionMode = ElementSelectionMode.Single;
         SeriesSelectionMode = SeriesSelectionMode.Point;
         ToolTipController = new ToolTipController {ToolTipType = ToolTipType.SuperTip};
         ToolTipController.Initialize(imageListRetriever);

         BoundDataChanged += (o, e) => this.DoWithinExceptionHandler(onBoundDataChanged);
         ObjectHotTracked += (o, e) => this.DoWithinExceptionHandler(() => onObjectHotTracked(e));
         ObjectSelected += (o, e) => this.DoWithinExceptionHandler(() => onObjectSelected(e));
      }

      public void ResetPlot()
      {
         if (XYDiagram == null)
            return;

         Series.Clear();
         SeriesDataMember = null;
         DataSource = null;
         SeriesTemplate.ArgumentDataMember = null;
         _title.Visibility = DefaultBoolean.False;
      }

      public void Plot(DiscreteDistributionData dataToPlot, DistributionSettings settings)
      {
         ResetPlot();
         plot(dataToPlot, settings);
         AxisX.WholeRange.AutoSideMargins = true;
         AxisX.WholeRange.SideMarginsValue = 0.5D;
      }

      public void Plot(ContinuousDistributionData distributionData, DistributionSettings settings)
      {
         ResetPlot();
         plot(distributionData, settings);

         if (!distributionData.IsRangeSpecified)
            return;

         var templateView = SeriesTemplate.View as BarSeriesView;
         if (templateView != null)
            templateView.BarWidth = distributionData.BarWidth;

         AxisX.WholeRange.SetMinMaxValues(distributionData.XMinValue, distributionData.XMaxValue);
      }

      private void plot<T>(DistributionData<T> dataToPlot, DistributionSettings settings)
      {
         DataSource = dataToPlot.DataTable;
         SeriesDataMember = dataToPlot.GroupingName;
         InitializeColor();
         SeriesTemplate.ArgumentDataMember = dataToPlot.XAxisName;
         SeriesTemplate.ArgumentScaleType = ScaleType.Auto;

         SeriesTemplate.ValueDataMembers.AddRange(dataToPlot.YAxisName);
         SeriesTemplate.ValueScaleType = ScaleType.Numerical;

         SeriesTemplate.LabelsVisibility = DefaultBoolean.False;
         SeriesTemplate.View = barViewFrom(settings.BarType);

         Legend.Visibility = Series.Count > 1 ? DefaultBoolean.True : DefaultBoolean.False;

         _title.Visibility = DefaultBoolean.True;
         _title.Text = settings.PlotCaption;

         updateDiagramFromSettings(settings);

         if (!dataToPlot.IsRangeSpecified)
            AxisX.WholeRange.Auto = true;

         AxisX.WholeRange.AutoSideMargins = false;
         AxisX.WholeRange.SideMarginsValue = 0;
      }

      private SeriesViewBase barViewFrom(BarType barType)
      {
         switch (barType)
         {
            case BarType.Stacked:
               return new StackedBarSeriesView();
            case BarType.SideBySide:
               return new SideBySideBarSeriesView();
            default:
               throw new ArgumentOutOfRangeException(nameof(barType));
         }
      }

      private void updateDiagramFromSettings(DistributionSettings settings)
      {
         AxisX.Title.Text = settings.XAxisTitle;
         AxisX.Title.Visibility = DefaultBoolean.True;
         AxisY.Title.Text = settings.YAxisTitle;
         AxisY.Title.Visibility = DefaultBoolean.True;
      }

      private void onObjectSelected(HotTrackEventArgs e)
      {
         e.Cancel = true;
      }

      private void onObjectHotTracked(HotTrackEventArgs e)
      {
         var series = e.Series();
         var point = e.HitInfo.SeriesPoint;
         if (series == null || point == null || !point.Values.Any())
         {
            hideToolTip(e);
            return;
         }

         var value = point.Values[0];
         var intValue = Math.Ceiling(point.Values[0]);
         var valueText = _doubleFormatter.Format(point.Values[0]);
         if (value == intValue)
            valueText = _intFormatter.Format(Convert.ToInt32(intValue));

         var superToolTip = _toolTipCreator.CreateToolTip($"{AxisY.Title.Text}: {valueText}", series.Name);
         var args = new ToolTipControllerShowEventArgs {SuperTip = superToolTip};
         ToolTipController.ShowHint(args);
      }

      private void onBoundDataChanged()
      {
         foreach (var barSeries in allBarSeries)
         {
            var barSeriesView = barSeries.View.DowncastTo<BarSeriesView>();
            barSeriesView.FillStyle.FillMode = FillMode.Gradient;
            barSeriesView.Color = StartColorFor(barSeries.Name);
            ((GradientFillOptionsBase) barSeriesView.FillStyle.Options).Color2 = EndColorFor(barSeries.Name);
         }
      }

      private IEnumerable<Series> allBarSeries
      {
         get
         {
            foreach (Series series in Series)
            {
               var barSeriesView = series.View as BarSeriesView;
               if (barSeriesView != null)
                  yield return series;
            }
         }
      }

      private void hideToolTip(HotTrackEventArgs e)
      {
         e.Cancel = true;
         ToolTipController.HideHint();
      }
   }
}