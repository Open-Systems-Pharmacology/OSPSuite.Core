using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX;
using OSPSuite.TeXReporting.TeX.Converter;
using OSPSuite.TeXReporting.TeX.PGFPlots;
using Plot = OSPSuite.TeXReporting.TeX.PGFPlots.Plot;

namespace OSPSuite.Infrastructure.Reporting.TeXBuilder
{
   public class ChartTeXBuilder : OSPSuiteTeXBuilder<CurveChart>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly IDimensionFactory _dimensionFactory;

      public ChartTeXBuilder(ITeXBuilderRepository builderRepository, IDimensionFactory dimensionFactory)
      {
         _builderRepository = builderRepository;
         _dimensionFactory = dimensionFactory;
      }

      private enum ChartTypes
      {
         XYPlot,
         XYY2Plot,
         XYY2Y3Plot
      }

      public override void Build(CurveChart chart, OSPSuiteTracker tracker)
      {
         if (!chart.Axes.Any()) return;

         var listToReport = new List<object>();

         AxisTypes firstOrdinate;
         AxisTypes secondOrdinate;
         AxisTypes thirdOrdinate;

         var colors = getUsedColors(chart);

         ChartTypes chartType = checkChartType(chart, out firstOrdinate, out secondOrdinate, out thirdOrdinate);

         switch (chartType)
         {
            case ChartTypes.XYPlot:
               {
                  var axisOptions = getAxisOptions(chart, firstOrdinate);
                  axisOptions.YAxisPosition = AxisOptions.AxisYLine.box;
                  var plots = getPlots(chart, firstOrdinate);

                  var plot = new OSPSuite.TeXReporting.Items.Plot(colors, axisOptions, plots, new Text(chart.Description))
                                {Position = FigureWriter.FigurePositions.H};
                  listToReport.Add(plot);
                  tracker.AddReference(chart, plot);
               }
               break;
            case ChartTypes.XYY2Plot:
               {
                  var axisOptionsY = getAxisOptions(chart, firstOrdinate);
                  var plotsY = getPlots(chart, firstOrdinate);
                  addOrdinateToLegendEntry(plotsY, firstOrdinate);

                  var axisOptionsY2 = getAxisOptions(chart, secondOrdinate);
                  axisOptionsY2.BackgroundColor = string.Empty;
                  var plotsY2 = getPlots(chart, secondOrdinate);
                  addOrdinateToLegendEntry(plotsY2, secondOrdinate);

                  var plot = new PlotTwoOrdinates(colors, axisOptionsY, plotsY, axisOptionsY2,
                                                  plotsY2, new Text(chart.Description)) 
                                                  { Position = FigureWriter.FigurePositions.H };

                  listToReport.Add(plot); 
                  tracker.AddReference(chart, plot);
               }
               break;
            case ChartTypes.XYY2Y3Plot:
               {
                  var axisOptionsY = getAxisOptions(chart, firstOrdinate);
                  var plotsY = getPlots(chart, firstOrdinate);
                  addOrdinateToLegendEntry(plotsY, firstOrdinate);

                  var axisOptionsY2 = getAxisOptions(chart, secondOrdinate);
                  axisOptionsY2.BackgroundColor = string.Empty;
                  var plotsY2 = getPlots(chart, secondOrdinate);
                  addOrdinateToLegendEntry(plotsY2, secondOrdinate);

                  var axisOptionsY3 = getAxisOptions(chart, thirdOrdinate);
                  axisOptionsY3.BackgroundColor = string.Empty;
                  var plotsY3 = getPlots(chart, thirdOrdinate);
                  addOrdinateToLegendEntry(plotsY3, thirdOrdinate);

                  var plot = new PlotThreeOrdinates(colors, axisOptionsY, plotsY, axisOptionsY2,
                                                    plotsY2, axisOptionsY3, plotsY3,
                                                    new Text(chart.Description)) 
                                                    { Position = FigureWriter.FigurePositions.H };

                  listToReport.Add(plot);
                  tracker.AddReference(chart, plot);
               }
               break;
         }

         _builderRepository.Report(listToReport, tracker);
      }

      private void addOrdinateToLegendEntry(IEnumerable<Plot> plots, AxisTypes ordinate)
      {
         foreach (var plot in plots) 
            plot.LegendEntry = $"({ordinate}) {plot.LegendEntry}";
      } 

      private ChartTypes checkChartType(CurveChart chart, out AxisTypes firstOrdinate, out AxisTypes secondOrdinate, out AxisTypes thirdOrdinate)
      {
         var YAxisUsed = isAxisTypeUsed(chart, AxisTypes.Y);
         var Y2AxisUsed = isAxisTypeUsed(chart, AxisTypes.Y2);
         var Y3AxisUsed = isAxisTypeUsed(chart, AxisTypes.Y3);

         // Single Ordinate
         if (YAxisUsed && !Y2AxisUsed && !Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y;
            secondOrdinate = AxisTypes.Y2;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYPlot;
         }
         if (!YAxisUsed && Y2AxisUsed && !Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y2;
            secondOrdinate = AxisTypes.Y2;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYPlot;
         }
         if (!YAxisUsed && !Y2AxisUsed && Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y3;
            secondOrdinate = AxisTypes.Y2;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYPlot;
         }

         // Two Ordinates
         if (YAxisUsed && Y2AxisUsed && !Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y;
            secondOrdinate = AxisTypes.Y2;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYY2Plot;
         }
         if (YAxisUsed && !Y2AxisUsed && Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y;
            secondOrdinate = AxisTypes.Y3;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYY2Plot;
         }
         if (!YAxisUsed && Y2AxisUsed && Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y2;
            secondOrdinate = AxisTypes.Y3;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYY2Plot;
         }

         // Three Ordinates
         if (YAxisUsed && Y2AxisUsed && Y3AxisUsed)
         {
            firstOrdinate = AxisTypes.Y;
            secondOrdinate = AxisTypes.Y2;
            thirdOrdinate = AxisTypes.Y3;
            return ChartTypes.XYY2Y3Plot;
         }
         throw new Exception("Unsupported chart type detected!");
      }

      private bool isAxisTypeUsed(CurveChart chart, AxisTypes yAxisType)
      {
         if (!chart.HasAxis(yAxisType)) 
            return false;

         var yAxis = chart.Axes.FirstOrDefault(x => x.AxisType == yAxisType);
         if (yAxis?.Dimension == null) 
            return false;

         return isAtLeastOneCurveCompatibleAndVisible(chart, yAxisType, yAxis);
      }

      private bool isAtLeastOneCurveCompatibleAndVisible(CurveChart chart, AxisTypes yAxisType, Axis yAxis)
      {
         return chart.Curves.Any(x => x.yAxisType == yAxisType && x.Visible && isCurveCompatibleToYAxis(x, yAxis));
      }

      private List<Color> getUsedColors(CurveChart chart)
      {
         var colors = new List<Color>();

         if (!colors.Contains(chart.ChartSettings.BackColor))
            colors.Add(chart.ChartSettings.BackColor);

         if (!colors.Contains(chart.ChartSettings.DiagramBackColor))
            colors.Add(chart.ChartSettings.DiagramBackColor);

         foreach (var curve in chart.Curves)
         {
            if (!colors.Contains(curve.Color))
               colors.Add(curve.Color);
         }
         return colors;
      }

      private string getAxisLabel(Axis axis)
      {
         var axisLabel = string.IsNullOrEmpty(axis.Caption)
                   ? $"{axis.Dimension}{(string.IsNullOrEmpty(axis.UnitName) ? string.Empty : $" [{axis.UnitName}]")}"
            : axis.Caption;
         if (axis.NumberMode == NumberModes.Relative)
            axisLabel += " in percentage";
         return axisLabel;
      }

      private LegendOptions.LegendPositions getLegendPosition(LegendPositions legendPosition)
      {
         switch (legendPosition)
         {
            case LegendPositions.None:
               return LegendOptions.LegendPositions.OuterSouth;
            case LegendPositions.Bottom:
               return LegendOptions.LegendPositions.OuterSouthEast;
            case LegendPositions.BottomInside:
               return LegendOptions.LegendPositions.SouthEast;
            case LegendPositions.Right:
               return LegendOptions.LegendPositions.OuterNorthEast;
            case LegendPositions.RightInside:
               return LegendOptions.LegendPositions.NorthEast;
            default:
               return LegendOptions.LegendPositions.NorthEast;
         }
      }

      private AxisOptions getAxisOptions(CurveChart chart, AxisTypes yAxisType)
      {
         var legendOptions = new LegendOptions
                                {
                                   LegendPosition = getLegendPosition(chart.ChartSettings.LegendPosition),
                                   LegendAlignment = LegendOptions.LegendAlignments.left,
                                   FontSize = LegendOptions.FontSizes.scriptsize,
                                   RoundedCorners = false
                                };

         var axisOptions = new AxisOptions(DefaultConverter.Instance)
                              {
                                 LegendOptions = legendOptions,
                                 Title = chart.Title,
                                 BackgroundColor = chart.ChartSettings.DiagramBackColor.Name,
                                 EnlargeLimits = chart.ChartSettings.SideMarginsEnabled
                              };

         foreach (var axis in chart.Axes)
         {
            if (axis.AxisType == AxisTypes.X)
            {
               axisOptions.XLabel = getAxisLabel(axis);
               axisOptions.XMax = axis.Max;
               axisOptions.XMin = axis.Min;
               var unit = axis.Dimension.Unit(axis.UnitName);

               axisOptions.XMajorGrid = axis.GridLines;
               axisOptions.XMode = getAxisMode(axis.Scaling);

               axisOptions.XAxisPosition = AxisOptions.AxisXLine.box;
               axisOptions.XAxisArrow = false;
            }
            else if (axis.AxisType == yAxisType)
            {
               axisOptions.YLabel = getAxisLabel(axis);
               axisOptions.YMax = axis.Max;
               axisOptions.YMin = axis.Min;

               axisOptions.YMajorGrid = axis.GridLines;
               axisOptions.YMode = getAxisMode(axis.Scaling);

               axisOptions.YAxisPosition = AxisOptions.AxisYLine.left;
               axisOptions.YAxisArrow = false;
            }
         }

         return axisOptions;
      }

      private AxisOptions.AxisMode getAxisMode(Scalings scaling)
      {
         switch(scaling)
         {
            case Scalings.Linear:
               return AxisOptions.AxisMode.normal;
            case Scalings.Log:
               return AxisOptions.AxisMode.log;
            default:
               throw new ArgumentOutOfRangeException(nameof(scaling));
         }
      }

      private float getMinXValue(CurveChart chart)
      {
         float min = float.MaxValue;
         foreach (var curve in chart.Curves)
         {
            if (!curve.Visible) continue;
            var curMin = curve.xData.Values.Min();
            if (min > curMin) min = curMin;
         }
         return min;
      }

      private float getMinYValue(CurveChart chart, AxisTypes axisType)
      {
         float min = float.MaxValue;
         foreach (var curve in chart.Curves)
         {
            if (curve.yAxisType != axisType) continue;
            if (!curve.Visible) continue;
            var curMin = curve.xData.Values.Min();
            if (min > curMin) min = curMin;
         }
         return min;
      }

      private float getMaxXValue(CurveChart chart)
      {
         float max = float.MinValue;
         foreach (var curve in chart.Curves)
         {
            if (!curve.Visible) continue;
            var curMax = curve.xData.Values.Max();
            if (max < curMax) max = curMax;
         }
         return max;
      }

      private float getMaxYValue(CurveChart chart, AxisTypes axisType)
      {
         float max = float.MinValue;
         foreach (var curve in chart.Curves)
         {
            if (curve.yAxisType != axisType) continue;
            if (!curve.Visible) continue;
            var curMax = curve.yData.Values.Max();
            if (max < curMax) max = curMax;
         }
         return max;
      }

      private double convertLineThickness(int lineThickness)
      {
         return 1 + 0.5*(lineThickness - 1);
      }

      private static string getOpacityFor(byte opacity)
      {
         var transparency = 255 - opacity;
         return (1 - transparency / 255D).ToString("0.00", CultureInfo.InvariantCulture);
      }

      private PlotOptions.Markers getMarker(Symbols symbol)
      {
         switch (symbol)
         {
            case Symbols.Circle:
               return PlotOptions.Markers.Circle;
            case Symbols.Diamond:
               return PlotOptions.Markers.Diamond;
            case Symbols.Square:
               return PlotOptions.Markers.Square;
            case Symbols.Triangle:
               return PlotOptions.Markers.Triangle;
            case Symbols.None:
               return PlotOptions.Markers.None;
         }
         return PlotOptions.Markers.None;
      }

      private PlotOptions.LineStyles getLineStyle(LineStyles lineStyle)
      {
         switch (lineStyle)
         {
            case LineStyles.None:
               return PlotOptions.LineStyles.None;
            case LineStyles.Solid:
               return PlotOptions.LineStyles.Solid;
            case LineStyles.Dash:
               return PlotOptions.LineStyles.Dashed;
            case LineStyles.DashDot:
               return PlotOptions.LineStyles.DashDotted;
            case LineStyles.Dot:
               return PlotOptions.LineStyles.Dotted;
         }
         return PlotOptions.LineStyles.None;
      }

      private bool isCurveCompatibleToYAxis(Curve curve, Axis yaxis)
      {
         return (curve.yDimension.Name == _dimensionFactory.MergedDimensionFor(yaxis).Name);
      }

      private List<Plot> getPlots(CurveChart chart, AxisTypes yAxisType)
      {
         var plots = new List<Plot>();

         var xAxis = chart.Axes.First(x => x.AxisType == AxisTypes.X);
         var yAxis = chart.Axes.First(x => x.AxisType == yAxisType);

         var xUnit = xAxis.Dimension.Unit(xAxis.UnitName);
         var yUnit = yAxis.Dimension.Unit(yAxis.UnitName);

         foreach (var curve in chart.Curves)
         {
            if (!curve.Visible) continue;
            if (curve.yAxisType != yAxisType) continue;
            if (!isCurveCompatibleToYAxis(curve, yAxis)) continue;

            var coordinates = new List<Coordinate>();
            var plotOptions = new PlotOptions();
            DataColumn geometricErr = null;
            if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricStdDev))
            {
               geometricErr = curve.yData.GetRelatedColumn(AuxiliaryType.GeometricStdDev);
               plotOptions.ErrorBars = true;
               plotOptions.ErrorType = PlotOptions.ErrorTypes.geometric;
            }
            DataColumn arithmeticErr = null;
            IDimension arithmeticErrDim = null;
            if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticStdDev))
            {
               arithmeticErr = curve.yData.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);
               arithmeticErrDim = _dimensionFactory.MergedDimensionFor(arithmeticErr);
               plotOptions.ErrorBars = true;
               plotOptions.ErrorType = PlotOptions.ErrorTypes.arithmetic;
            }
            DataColumn geometricMeanPop = null;
            IDimension geometricMeanPopDim = null;
            if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
            {
               geometricMeanPop = curve.yData.GetRelatedColumn(AuxiliaryType.GeometricMeanPop);
               geometricMeanPopDim = _dimensionFactory.MergedDimensionFor(geometricMeanPop);
               plotOptions.ErrorBars = false;
               plotOptions.ErrorType = PlotOptions.ErrorTypes.geometric;
               plotOptions.ShadedErrorBars = true;
               plotOptions.Opacity = getOpacityFor(Constants.RANGE_AREA_OPACITY);
            }
            DataColumn arithmeticMeanPop = null;
            IDimension arithmeticMeanPopDim = null;
            if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
            {
               arithmeticMeanPop = curve.yData.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);
               arithmeticMeanPopDim = _dimensionFactory.MergedDimensionFor(arithmeticMeanPop);
               plotOptions.ErrorBars = false;
               plotOptions.ErrorType = PlotOptions.ErrorTypes.arithmetic;
               plotOptions.ShadedErrorBars = true;
               plotOptions.Opacity = getOpacityFor(Constants.RANGE_AREA_OPACITY);
            }

            plotOptions.Color = curve.Color.Name;
            plotOptions.ThicknessSize = Helper.Length(convertLineThickness(curve.LineThickness), Helper.MeasurementUnits.pt);
            plotOptions.MarkSize = Helper.Length(convertLineThickness(curve.LineThickness), Helper.MeasurementUnits.pt);

            plotOptions.Marker = getMarker(curve.Symbol);
            plotOptions.LineStyle = getLineStyle(curve.LineStyle);

            IDimension xDimension = _dimensionFactory.MergedDimensionFor(curve.xData);
            IDimension yDimension = _dimensionFactory.MergedDimensionFor(curve.yData);
            for (var i = 0; i < curve.xData.Values.Count; i++)
            {
               var xValue = convertToUnit(xUnit, xDimension, curve.xData.Values[i]);
               var yValue = convertToUnit(yUnit, yDimension, curve.yData.Values[i]);

               var coordinate = new Coordinate(xValue,yValue);

               if (geometricMeanPop != null)
               {
                  var yError = curve.yData.Values[i];
                  yValue = convertToUnit(yUnit, geometricMeanPopDim, geometricMeanPop.Values[i]);
                  coordinate = new Coordinate(coordinate.X, yValue) {errY = yError};
               }
               if (arithmeticMeanPop != null)
               {
                  var yError = yValue; 
                  yValue = convertToUnit(yUnit, arithmeticMeanPopDim, arithmeticMeanPop.Values[i]);
                  coordinate = new Coordinate(coordinate.X, yValue) {errY = yError};
               }

               if (geometricErr != null)
                  coordinate.errY = geometricErr.Values[i];
               if (arithmeticErr != null)
                  coordinate.errY = convertToUnit(yUnit, arithmeticErrDim, arithmeticErr.Values[i]);

               if (xAxis.NumberMode == NumberModes.Relative)
               {
                  var max = curve.xData.Values.Max();
                  max = convertToUnit(xUnit, xDimension, max);
                  coordinate = new Coordinate(coordinate.X / max * 100, coordinate.Y) {errY =  coordinate.errY};
               }

               if (yAxis.NumberMode == NumberModes.Relative)
               {
                  var max = curve.yData.Values.Max();
                  if (geometricMeanPop != null)
                  {
                     max = geometricMeanPop.Values.Max();
                     max = convertToUnit(yUnit, geometricMeanPopDim, max);
                  }
                  if (arithmeticMeanPop != null)
                  {
                     max = arithmeticMeanPop.Values.Max();
                     max = convertToUnit(yUnit, arithmeticMeanPopDim, max);
                  }
                  coordinate = new Coordinate(coordinate.X, coordinate.Y/max*100)
                                  {
                                     errY =
                                        coordinate.errY == null
                                           ? null
                                           : (arithmeticErr != null | arithmeticMeanPop != null)
                                                ? coordinate.errY/max*100
                                                : coordinate.errY
                                  };
               }

               coordinates.Add(coordinate);
            }

            var plot = new Plot(coordinates, plotOptions) {LegendEntry = curve.Name};

            // if all coordinates are nan, no plot can be created, so skip this plot
            if (!coordinates.All(c => float.IsNaN(c.X)) && !coordinates.All(c => float.IsNaN(c.Y)))
               plots.Add(plot);
         }

         return plots;
      }

      private float convertToUnit(Unit unit, IDimension dimension, float value)
      {
         try
         {
            return (float) dimension.BaseUnitValueToUnitValue(unit ?? dimension.DefaultUnit, value);
         }
         catch (UnableToResolveParametersException)
         {
         }
         return float.NaN;
      }
   }
}