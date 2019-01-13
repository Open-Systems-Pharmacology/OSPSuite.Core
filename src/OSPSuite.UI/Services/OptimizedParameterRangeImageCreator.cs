using System;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Services
{
   public class OptimizedParameterRangeImageCreator : IOptimizedParameterRangeImageCreator
   {
      private readonly UxChartControl _chart;

      private Size _chartSize = new Size(UIConstants.Size.OPTIMIZED_RANGE_WIDTH, 100);

      public OptimizedParameterRangeImageCreator()
      {
         _chart = new UxChartControl {Size = _chartSize};
         _chart.Legend.Visibility = DefaultBoolean.False;
      }

      public Image CreateFor(OptimizedParameterDTO optimizedParameterDTO)
      {
         _chart.Series.Clear();
         var minValue = getValueAccordingToScaling(optimizedParameterDTO.MinValue.Value, optimizedParameterDTO.Scaling);
         var maxValue = getValueAccordingToScaling(optimizedParameterDTO.MaxValue.Value, optimizedParameterDTO.Scaling);
         var startValue = getValueAccordingToScaling(optimizedParameterDTO.StartValue.Value, optimizedParameterDTO.Scaling);
         var optimalValue = getValueAccordingToScaling(optimizedParameterDTO.OptimalValue.Value, optimizedParameterDTO.Scaling);

         createPointSeries("seriesBoundary", MarkerKind.Diamond, minValue, maxValue).LegendText = Captions.Boundary;
         createPointSeries("seriesOptimal", MarkerKind.Circle, optimalValue).LegendText = Captions.OptimalValue;
         createPointSeries("seriesStart", MarkerKind.Square, startValue).LegendText = Captions.StartValue;

         _chart.XYDiagram.AxisY.Visibility = DefaultBoolean.False;
         _chart.XYDiagram.AxisY.WholeRange.AutoSideMargins = false;
         _chart.XYDiagram.DefaultPane.BorderVisible = false;

         var image = new Bitmap(_chartSize.Width, _chartSize.Height);
         
         _chart.DrawToBitmap(image, new Rectangle(Point.Empty, _chartSize));
         return image;
      }

      public Image CreateLegendFor(OptimizedParameterDTO optimizedParameterDTO)
      {
         CreateFor(optimizedParameterDTO);

         var legendBitmap = new Bitmap(110, 55);
         using (var clonedChart = (ChartControl)_chart.Clone())
         {
            clonedChart.Legend.Visibility= DefaultBoolean.True;
            clonedChart.BorderOptions.Visibility = DefaultBoolean.False;
            clonedChart.Padding.All = 0;
            clonedChart.Legend.Border.Visibility = DefaultBoolean.False;
            clonedChart.Legend.Margins.All = 0;
            clonedChart.Legend.Padding.All = 0;
            clonedChart.Legend.BackColor = Color.Transparent;
            clonedChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.LeftOutside;
            clonedChart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;

            ((XYDiagram)clonedChart.Diagram).DefaultPane.Visibility =ChartElementVisibility.Hidden;
            clonedChart.DrawToBitmap(legendBitmap, new Rectangle(Point.Empty, legendBitmap.Size));
         }
         return legendBitmap;
      }


      private static double getValueAccordingToScaling(double value, Scalings scaling)
      {
         return scaling == Scalings.Log ? Math.Log(value) : value;
      }

      private Series createPointSeries(string name, MarkerKind markerKind, params double[] points)
      {
         var series = new Series(name, ViewType.Point) {ArgumentScaleType = ScaleType.Numerical};
         points.Each(p => series.Points.Add(new SeriesPoint(p, 0)));

         var view = series.View.DowncastTo<PointSeriesView>();
         view.PointMarkerOptions.Kind = markerKind;

         _chart.Series.Add(series);

         return series;
      }
   }
}