using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Utility.Extensions;
using Axis = OSPSuite.Core.Chart.Axis;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   internal abstract class CurveBinder : ICurveBinder
   {
      private readonly ChartControl _chartControl;
      private readonly AxisYBase _axisView;
      private readonly DataMode _dataMode;
      private readonly FillMode SeriesMarkerFillMode = FillMode.Solid;
      private readonly Axis _xAxis;
      private readonly Axis _yAxis;

      private readonly DataTable _dataTable;
      protected readonly List<Series> _series;
      private readonly AxisTypes _yAxisType;

      private const string X = "X";
      protected const string Y = "Y";
      private const string Y2 = "Y2";
      private const string LOW = "Low";
      private const string HIGH = "High";
      private const string LLOQ_SUFFIX = "LLOQ";
      private const string INDEX_OF_VALUE_IN_CURVE = "IndexOfValueInCurve";

      protected CurveBinder(Curve curve, ChartControl chartControl, CurveChart chart, AxisYBase yAxisView, DataMode dataMode)
      {
         _chartControl = chartControl;
         _axisView = yAxisView;
         _dataMode = dataMode;
         Curve = curve;
         _xAxis = chart.AxisBy(AxisTypes.X);
         _yAxis = chart.AxisBy(curve.yAxisType);
         _yAxisType = curve.yAxisType;
         _dataTable = new DataTable(Curve.Id);
         _series = new List<Series>();

         initializeData();
      }

      public int? LegendIndex => Curve.LegendIndex;

      public Curve Curve { get; }

      public void Dispose()
      {
         _dataTable.Dispose();

         foreach (var series in _series)
         {
            _chartControl.Series.Remove(series);
            series.Dispose();
         }
      }

      private void initializeData()
      {
         createData();
         refreshData();
         createAndBindSeries();
      }

      /// <summary>
      ///    Implementation specific  Y columns to be added to the chart. Y always be added
      /// </summary>
      protected abstract IReadOnlyList<string> YColumnNames { get; }

      private void createData()
      {
//         switch (dataMode)
//         {
//            case DataModes.SingleValue:
//               yColumnNames.Add(Y);
//               break;
//
//            case DataModes.StdDevG:
//               yColumnNames.Add(LOW);
//               yColumnNames.Add(HIGH);
//               yColumnNames.Add(Y);
//               yColumnNames.Add(Y2);
//               break;
//
//            case DataModes.StdDevA:
//               yColumnNames.Add(LOW);
//               yColumnNames.Add(HIGH);
//               yColumnNames.Add(Y);
//               yColumnNames.Add(Y2);
//               break;
//
//            case DataModes.StdDevGPop:
//               yColumnNames.Add(LOW);
//               yColumnNames.Add(HIGH);
//               yColumnNames.Add(Y);
//               break;
//
//            case DataModes.StdDevAPop:
//               yColumnNames.Add(LOW);
//               yColumnNames.Add(HIGH);
//               yColumnNames.Add(Y);
//               break;
//         }

         //Always added for each curve
         _dataTable.AddColumn<float>(X);

         _dataTable.AddColumns<float>(YColumnNames);

         if (HasLLOQ)
            _dataTable.AddColumns<float>(LLOQ_SUFFIX);

         _dataTable.AddColumn<int>(INDEX_OF_VALUE_IN_CURVE);
      }

      public bool HasLLOQ => Curve.yData.DataInfo.LLOQ.HasValue;

      public IEnumerable<string> SeriesIds => _series.Select(series => series.Name);

      public double? LLOQ
      {
         get
         {
            if (!HasLLOQ)
               return null;
            return Curve.YDimension.BaseUnitValueToUnitValue(Curve.YDimension.Unit(_yAxis.UnitName), Convert.ToDouble(Curve.yData.DataInfo.LLOQ));
         }
      }

      private void createAndBindSeries()
      {
         var series = createSeries(Curve.Id, ViewType.ScatterLine, YColumnNames.ToArray());

         PopulateSeries(series);

         if (HasLLOQ)
            createLowerLimitOfQuantificationSeries();
         //
         //         if (dataMode == DataModes.StdDevA || dataMode == DataModes.StdDevG)
         //         {
         //            //add additional series for points and line and upper and lower markers
         //            series.ShowInLegend = Curve.VisibleInLegend;
         //            createPointLineSeries();
         //            createUpperPointSeries();
         //            createLowerPointSeries();
         //         }
         //
         //         if (dataMode == DataModes.StdDevAPop || dataMode == DataModes.StdDevGPop)
         //         {
         //            createPointLineSeries();
         //         }

         attachYAxisToSeries();

         _chartControl.Series.AddRange(_series.ToArray());
      }

      protected virtual void PopulateSeries(Series series)
      {
         //implement specific behavior of required
      }

      private void createPointLineSeries()
      {
         var series = createSeries($"{Curve.Id}_Line", ViewType.ScatterLine, Y);
         series.ShowInLegend = true;
         series.Visible = Curve.CurveOptions.IsReallyVisible;
      }

      private void createLowerLimitOfQuantificationSeries()
      {
         var series = createSeries<LineSeriesView>($"{Curve.Id}_{LLOQ_SUFFIX}", ViewType.Line, LLOQ_SUFFIX, lineSeriesView =>
         {
            lineSeriesView.LineStyle.DashStyle = DashStyle.Dot;
            lineSeriesView.LineStyle.Thickness = 2;
            lineSeriesView.MarkerVisibility = DefaultBoolean.False;
         });
         series.Visible = Curve.ShowLLOQ && Curve.Visible;
      }

      private void createUpperPointSeries()
      {
         createSeries<PointSeriesView>($"{Curve.Id}_Upper", ViewType.Point, HIGH, pointView =>
         {
            pointView.PointMarkerOptions.Kind = MarkerKind.InvertedTriangle;
            pointView.PointMarkerOptions.Size = 4;
         });
      }

      private void createLowerPointSeries()
      {
         createSeries<PointSeriesView>($"{Curve.Id}_Lower", ViewType.Point, LOW, pointView =>
         {
            pointView.PointMarkerOptions.Kind = MarkerKind.Triangle;
            pointView.PointMarkerOptions.Size = 4;
         });
      }

      private Series createSeries(string name, ViewType viewType, string valueDataMember)
      {
         return createSeries(name, viewType, new[] {valueDataMember});
      }

      private Series createSeries(string name, ViewType viewType, string[] valueDataMembers)
      {
         return createSeries<SeriesViewBase>(name, viewType, valueDataMembers);
      }

      private Series createSeries<TSeriesView>(string name, ViewType viewType, string valueDataMember, Action<TSeriesView> configuration = null) where TSeriesView : SeriesViewBase
      {
         return createSeries(name, viewType, new[] {valueDataMember}, configuration);
      }

      private Series createSeries<TSeriesView>(string name, ViewType viewType, string[] valueDataMembers, Action<TSeriesView> configuration = null) where TSeriesView : SeriesViewBase
      {
         var series = new Series(name, viewType)
         {
            ShowInLegend = false,
            LegendText = Curve.Name,
            DataSource = _dataTable,
            ArgumentScaleType = ScaleType.Numerical,
            ArgumentDataMember = X,
            ValueScaleType = ScaleType.Numerical,
            LabelsVisibility = DefaultBoolean.False,
            Visible = Curve.Visible
         };

         if (viewType == ViewType.ScatterLine)
            configureScatterLineView(series.View as ScatterLineSeriesView);

         series.ValueDataMembers.AddRange(valueDataMembers);

         var view = series.View as TSeriesView;
         if (configuration != null && view != null)
         {
            configuration(view);
         }

         _series.Add(series);

         return series;
      }

      private void configureScatterLineView(ScatterLineSeriesView scatterLineSeriesView)
      {
         if (scatterLineSeriesView == null)
            return;
         scatterLineSeriesView.EnableAntialiasing = DefaultBoolean.True;
      }

      private void attachYAxisToSeries()
      {
         foreach (var series in _series)
         {
            var xySeriesView = series.View as XYDiagramSeriesViewBase;
            if (xySeriesView != null)
               xySeriesView.AxisY = _axisView;
         }
      }

      public string Id => Curve.Id;

      public void Refresh()
      {
         refreshData();
         refreshView();
      }

      private void onAxisPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var axis = sender as Axis;
         if (axis == null) return;

         if (!axisPropertyIsCoordinateSystemRelevant(e.PropertyName))
            return;

         Refresh();
      }

      private bool axisPropertyIsCoordinateSystemRelevant(string propertyName)
      {
         return (propertyName == Helpers.Property<Axis>(a => a.UnitName).Name
                 || propertyName == Helpers.Property<Axis>(a => a.Scaling).Name
                 || propertyName == Helpers.Property<Axis>(a => a.NumberMode).Name);
      }

      private void onCurvePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var curve = sender as Curve;
         if (curve == null) return;

         if (e.PropertyName == Helpers.Property<Curve>(c => c.Name).Name)
         {
            foreach (var s in _series)
            {
//               s.LegendText = getLegendText();
            }
         }
         else if (e.PropertyName == Helpers.Property<Curve>(c => c.xData).Name
                  || e.PropertyName == Helpers.Property<Curve>(c => c.yData).Name
                  || e.PropertyName == Helpers.Property<Curve>(c => c.InterpolationMode).Name)
         {
            refreshData();
         }
         else if (e.PropertyName == Helpers.Property<Curve>(c => c.yAxisType).Name)
         {
            _yAxis.PropertyChanged -= onAxisPropertyChanged;
//            _yAxis = _axisAdapters[curve.yAxisType].Axis;
            _yAxis.PropertyChanged += onAxisPropertyChanged;

            foreach (var series in _series)
            {
//               series.LegendText = getLegendText();
               var xySeriesView = series.View as XYDiagramSeriesViewBase;
//               if (xySeriesView != null)
//                  xySeriesView.AxisY = _axisAdapters[curve.yAxisType].AxisView as AxisYBase;
            }

            refreshData();
         }

         refreshView();
      }

      private void refreshView()
      {
         RefreshSeries();
//
//         switch (dataMode)
//         {
//            case DataModes.Invalid:
//               throw new InvalidArgumentException($"Invalid RelatedColumns for {Curve.yData.Name}");
//
//            case DataModes.SingleValue:
//
//               SetSeriesOptionsForSingleValue(series);
//               break;
//
//            case DataModes.StdDevG:
//            case DataModes.StdDevA:
//               if (!seriesIsCandleStick(series))
//               {
//                  series.ChangeView(ViewType.CandleStick);
//               }
//
//               var financialSeriesView = series.View as FinancialSeriesViewBase;
//               if (financialSeriesView != null)
//               {
//                  financialSeriesView.ReductionOptions.Visible = false;
//                  financialSeriesView.LevelLineLength = 0;
//                  financialSeriesView.LineThickness = 1;
//                  financialSeriesView.Color = curveOptions.Color;
//               }
//
//               updateLinkedSeries(series);
//               break;
//
//            case DataModes.StdDevGPop:
//            case DataModes.StdDevAPop:
//               if (!seriesIsRangeArea(series))
//               {
//                  series.ChangeView(ViewType.RangeArea);
//               }
//
//               var rangeAreaSeriesView = series.View as RangeAreaSeriesView;
//               if (rangeAreaSeriesView != null)
//               {
//                  rangeAreaSeriesView.Marker1Visibility = DefaultBoolean.False;
//                  rangeAreaSeriesView.Marker2Visibility = DefaultBoolean.False;
//                  rangeAreaSeriesView.FillStyle.FillMode = FillMode.Solid;
//                  rangeAreaSeriesView.Color = curveOptions.Color;
//                  rangeAreaSeriesView.Transparency = Constants.Population.STD_DEV_CURVE_TRANSPARENCY;
//               }
//
//               updateLinkedSeries(series);
//               break;
//         }

         updateSeriesCaption();
         setLowerLimitOfQuantificationOptions();
         ShowCurveInLegend(Curve.VisibleInLegend);
      }

      private void updateSeriesCaption()
      {
         _series.Each(s => s.LegendText = Curve.Name);
      }

      protected abstract void RefreshSeries();

      private void updateLinkedSeries(Series series)
      {
         var curveOptions = Curve.CurveOptions;
         updateSeriesVisibility(visible: curveOptions.Visible);

         foreach (var s in _series.Except(new[] {series}))
         {
            var xySeriesView = s.View as XYDiagramSeriesViewBase;
            if (xySeriesView != null)
               xySeriesView.Color = curveOptions.Color;
         }

         //If we are dealing with more than one series, series at index 1 represents the series we need to deal with
         if (_series.Count > 1)
            SetSeriesOptionsForSingleValue(_series[1]);
      }

      private void setLowerLimitOfQuantificationOptions()
      {
         _series.Where(s => IsSeriesLLOQ(s.Name)).Each(setOptionsForLowerLimitOfQuantification);
      }

      private void setOptionsForLowerLimitOfQuantification(Series lloqSeries)
      {
         var curveOptions = Curve.CurveOptions;

         lloqSeries.Visible = curveOptions.ShouldShowLLOQ && curveOptions.Visible;

         var xySeriesView = lloqSeries.View as XYDiagramSeriesViewBase;
         if (xySeriesView != null)
         {
            xySeriesView.Color = curveOptions.Color;
         }
      }

      private void setLegendVisibilityForCandleStickView(bool showInLegend)
      {
         //we want to show the curve symbol in the legend instead of the line.
         _series[1].ShowInLegend = showInLegend;
         _series[0].ShowInLegend = false;
      }

      private bool seriesIsScatterLine(Series series) => seriesIs<ScatterLineSeriesView>(series);
      private bool seriesIsPoint(Series series) => seriesIs<PointSeriesView>(series);
      private bool seriesIsRangeArea(Series series) => seriesIs<RangeAreaSeriesView>(series);
      private bool seriesIsCandleStick(Series series) => seriesIs<CandleStickSeriesView>(series);

      private bool seriesIs<TSeries>(Series series) where TSeries : SeriesViewBase => series.View.GetType() == typeof(TSeries);

      protected void SetSeriesOptionsForSingleValue(Series series)
      {
         var curveOptions = Curve.CurveOptions;

         if (curveOptions.LineStyle == LineStyles.None & seriesIsScatterLine(series))
            series.ChangeView(ViewType.Point);

         else if (curveOptions.LineStyle != LineStyles.None & seriesIsPoint(series))
            series.ChangeView(ViewType.ScatterLine);

         series.Visible = curveOptions.IsReallyVisible && dimensionsConsistentToAxisUnits();

         var xySeriesView = series.View as XYDiagramSeriesViewBase;
         if (xySeriesView != null)
            xySeriesView.Color = curveOptions.Color;

         var pointSeriesView = series.View as PointSeriesView;
         if (pointSeriesView != null)
         {
            pointSeriesView.PointMarkerOptions.Kind = mapFrom(curveOptions.Symbol);
            pointSeriesView.PointMarkerOptions.Size = DataChartConstants.Display.SeriesMarkerSize + Curve.LineThickness * Curve.LineThickness;
            pointSeriesView.PointMarkerOptions.FillStyle.FillMode = SeriesMarkerFillMode;
         }

         if (seriesIsScatterLine(series))
         {
            var lineSeriesView = series.View as LineSeriesView;
            if (lineSeriesView != null)
            {
               if (curveOptions.LineStyle != LineStyles.None)
                  lineSeriesView.LineStyle.DashStyle = mapFrom(curveOptions.LineStyle);

               lineSeriesView.LineStyle.Thickness = curveOptions.LineThickness;
               lineSeriesView.MarkerVisibility = (curveOptions.Symbol != Symbols.None)
                  ? DefaultBoolean.True
                  : DefaultBoolean.False;

               lineSeriesView.LineMarkerOptions.FillStyle.FillMode = SeriesMarkerFillMode;
               lineSeriesView.LineMarkerOptions.Size = DataChartConstants.Display.SeriesMarkerSize + Curve.LineThickness * Curve.LineThickness;
            }
         }
      }

      private static MarkerKind mapFrom(Symbols symbol)
      {
         switch (symbol)
         {
            case Symbols.None:
               return MarkerKind.Square;
            case Symbols.Circle:
               return MarkerKind.Circle;
            case Symbols.Diamond:
               return MarkerKind.Diamond;
            case Symbols.Triangle:
               return MarkerKind.Triangle;
            case Symbols.Square:
               return MarkerKind.Square;
            default:
               throw new ArgumentOutOfRangeException(nameof(symbol));
         }
      }

      private static DashStyle mapFrom(LineStyles lineStyle)
      {
         switch (lineStyle)
         {
            case LineStyles.None:
               return DashStyle.Empty;
            case LineStyles.Solid:
               return DashStyle.Solid;
            case LineStyles.Dash:
               return DashStyle.Dash;
            case LineStyles.Dot:
               return DashStyle.Dot;
            case LineStyles.DashDot:
               return DashStyle.DashDot;
            default:
               throw new ArgumentOutOfRangeException(nameof(lineStyle));
         }
      }

      private void refreshData()
      {
         _dataTable.Clear();

         if (!dimensionsConsistentToAxisUnits()) return; //show no values

         var xDimension = Curve.XDimension;
         var yDimension = Curve.YDimension;
         var xUnit = xDimension.Unit(_xAxis.UnitName);
         var yUnit = yDimension.Unit(_yAxis.UnitName);
         var xData = Curve.xData;
         var yData = ActiveYData;
         var baseGrid = activeBaseGrid(xData, yData);

         // works for different base grids
         _dataTable.BeginLoadData();
         foreach (var baseValue in baseGrid.Values)
         {
            try
            {
               double x = xDimension.BaseUnitValueToUnitValue(xUnit, xData.GetValue(baseValue));
               double y = yDimension.BaseUnitValueToUnitValue(yUnit, yData.GetValue(baseValue));

               if (!isValidXValue(x) || !isValidYValue(y))
                  continue;

               var row = _dataTable.NewRow();
               row[X] = x;
               row[Y] = y;
               row[INDEX_OF_VALUE_IN_CURVE] = baseGrid.IndexOf(baseValue);

               if (HasLLOQ)
                  row[LLOQ_SUFFIX] = LLOQ;


               AddRelatedValuesToRow(row, yData, yDimension, yUnit, y, baseValue);
//               switch (mode)
//               {
//                  case DataModes.SingleValue:
//                     break;
//                  case DataModes.StdDevA:
//                     relatedColumn = yData.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);
//                     stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, relatedColumn.GetValue(baseValue));
//                     if (!isValidValue(stdDev))
//                        stdDev = 0;
//
//                     row[HIGH] = y + stdDev;
//                     row[LOW] = y - stdDev;
//                     row[Y2] = y;
//
//                     if (!isValidYValue(y - stdDev))
//                        row[LOW] = y;
//
//                     break;
//                  case DataModes.StdDevG:
//                     relatedColumn = yData.GetRelatedColumn(AuxiliaryType.GeometricStdDev);
//                     stdDev = relatedColumn.GetValue(baseValue);
//                     if (!isValidValue(stdDev) || stdDev == 0)
//                        stdDev = 1;
//
//                     if (!isValidYValue(y / stdDev))
//                        continue;
//
//                     row[LOW] = y / stdDev;
//                     row[HIGH] = y * stdDev;
//                     row[Y2] = y;
//
//                     break;
//                  case DataModes.StdDevAPop:
//                     stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, Curve.yData.GetValue(baseValue));
//                     if (!isValidValue(stdDev))
//                        stdDev = 0;
//
//                     if (!isValidYValue(y - stdDev))
//                        continue;
//
//                     row[LOW] = y - stdDev;
//                     row[HIGH] = y + stdDev;
//
//                     break;
//                  case DataModes.StdDevGPop:
//                     stdDev = Curve.yData.GetValue(baseValue);
//                     if (!isValidValue(stdDev) || stdDev == 0)
//                        stdDev = 1;
//
//                     if (!isValidYValue(y / stdDev))
//                        continue;
//
//                     row[LOW] = y / stdDev;
//                     row[HIGH] = y * stdDev;
//                     break;
//                  default:
//                     throw new InvalidArgumentException("Invalid RelatedColumns for " + yData.Name);
//               }

               _dataTable.Rows.Add(row);
            }
            catch (ArgumentOutOfRangeException)
            {
               //can  happen when plotting X vs Y and using different base grid
            }
         }

         if (_xAxis.NumberMode == NumberModes.Relative)
            setRelativeValues(X);

         if (_yAxis.NumberMode == NumberModes.Relative)
            setRelativeValues(Y);

         _dataTable.EndLoadData();
      }

      protected abstract void AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue);

      private BaseGrid activeBaseGrid(DataColumn xData, DataColumn yData)
      {
         if (Curve.CurveOptions.InterpolationMode == InterpolationModes.xLinear)
            return xData.BaseGrid;

         if (Curve.CurveOptions.InterpolationMode == InterpolationModes.yLinear)
            return yData.BaseGrid;

         throw new ArgumentException("InterpolationMode = " + Curve.InterpolationMode);
      }

      protected abstract DataColumn ActiveYData { get; }
//      {
//         if (dataMode == DataModes.StdDevAPop)
//            return Curve.yData.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);
//
//         if (dataMode == DataModes.StdDevGPop)
//            return Curve.yData.GetRelatedColumn(AuxiliaryType.GeometricMeanPop);
//
//         return Curve.yData;
//      }

      private bool isValidXValue(double x)
      {
         return isValidAxisValue(x, _xAxis);
      }

      private bool isValidYValue(double y)
      {
         return isValidAxisValue(y, _yAxis);
      }

      private bool isValidAxisValue(double value, Axis axis)
      {
         if (!isValidValue(value))
            return false;

         return axis.Scaling == Scalings.Linear || value > 0;
      }

      private static bool isValidValue(double value)
      {
         return !double.IsInfinity(value) && !double.IsNaN(value);
      }

      private void setRelativeValues(string columnName)
      {
         var max = GetMax(columnName);
         if (max <= 0) return;

         foreach (DataRow row in _dataTable.Rows)
         {
            if (row[columnName] != DBNull.Value)
               row[columnName] = (float) row[columnName] / max;

            if (columnName != Y)
               continue;

            SetRelatedRelativeValuesForRow(row, max);


//            switch (dataMode)
//            {
//               case DataModes.StdDevG:
//               case DataModes.StdDevA:
//                  if (row[Y2] != DBNull.Value)
//                     row[Y2] = (float) row[Y2] / max;
//                  if (row[LOW] != DBNull.Value)
//                     row[LOW] = (float) row[LOW] / max;
//                  if (row[HIGH] != DBNull.Value)
//                     row[HIGH] = (float) row[HIGH] / max;
//                  break;
//               case DataModes.StdDevGPop:
//               case DataModes.StdDevAPop:
//                  if (row[HIGH] != DBNull.Value)
//                     row[HIGH] = (float) row[HIGH] / max;
//                  break;
//            }
         }
      }

      protected abstract void SetRelatedRelativeValuesForRow(DataRow row, float max);

      protected virtual float GetMax(string columnName)
      {
         float max = 0;
         DataRow maxRow = null;
         foreach (DataRow row in _dataTable.Rows)
         {
            if (row[columnName] == DBNull.Value)
               continue;

            var value = (float) row[columnName];
            if (!double.IsNaN(value) && value > max)
            {
               max = value;
               maxRow = row;
            }
         }

         //TODO OVERRIDE IN CurveBinder for STDDevGPOP and STDDevvAPOP
//         if (columnName == Y && maxRow != null)
//         {
//            if (dataMode == DataModes.StdDevAPop)
//               max = ((float) maxRow[LOW] + (float) maxRow[HIGH]) / 2;
//            if (dataMode == DataModes.StdDevGPop)
//               max = (float) Math.Sqrt(((float) maxRow[LOW]) * ((float) maxRow[HIGH]));
//         }
         return max;
      }

      private bool dimensionsConsistentToAxisUnits()
      {
         return Curve.XDimension.CanConvertToUnit(_xAxis.UnitName) &&
                Curve.YDimension.CanConvertToUnit(_yAxis.UnitName);
      }

      public void ShowAllSeries()
      {
         updateSeriesVisibility(visible: true);
      }

      private void updateSeriesVisibility(bool visible)
      {
         _series.Each(s => s.Visible = visible);
      }

      public void ShowCurveInLegend(bool visibleInLegend)
      {
         if (isCandleStickView())
            setLegendVisibilityForCandleStickView(visibleInLegend);
         else
            _series[0].ShowInLegend = visibleInLegend;
      }

      private bool isCandleStickView()
      {
         return _series.Count(series => !IsSeriesLLOQ(series.Name)) > 1;
      }

      public bool ContainsSeries(string id)
      {
         return _series.Any(s => string.Equals(s.Name, id));
      }

      public bool IsSeriesLLOQ(string seriesId)
      {
         return !string.IsNullOrEmpty(seriesId) && seriesId.EndsWith($"_{LLOQ_SUFFIX}");
      }

      public int OriginalCurveIndexForRow(DataRow row)
      {
         return (int) row[INDEX_OF_VALUE_IN_CURVE];
      }

      public bool IsValidFor(DataMode dataMode, AxisTypes yAxisType)
      {
         return _dataMode == dataMode &&
                _yAxisType == yAxisType;
      }
   }
}