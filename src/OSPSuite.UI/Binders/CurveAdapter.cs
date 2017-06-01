using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Presenters.Charts;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.UI.Binders
{
   /// <summary>
   ///    Adapter from Curve and its two Axes (DataChart.Data) to a Series and SeriesView (DevExpress.XtraCharts).
   /// </summary>
   internal class CurveAdapter : ICurveAdapter
   {
      public static FillMode SeriesMarkerFillMode = FillMode.Solid;
      private ICache<AxisTypes, IAxisAdapter> _axisAdapters;
      private readonly IAxis _xAxis;
      private IAxis _yAxis;

      private readonly DataTable _dataTable;
      private readonly List<Series> _series;

      private const string X = "X";
      private const string Y = "Y";
      private const string Y2 = "Y2";
      private const string LOW = "Low";
      private const string HIGH = "High";
      private const string LLOQ_SUFFIX = "LLOQ";
      private const string INDEX_OF_VALUE_IN_CURVE = "CurveIndex";

      public CurveAdapter(ICurve curve, IAxis xAxis, IAxis yAxis)
      {
         Curve = curve;
         _xAxis = xAxis;
         _yAxis = yAxis;

         Curve.PropertyChanged += onCurvePropertyChanged;
         _xAxis.PropertyChanged += onAxisPropertyChanged;
         _yAxis.PropertyChanged += onAxisPropertyChanged;

         _dataTable = new DataTable(Curve.Name);
         _series = new List<Series>();
         initializeData();

         refreshView();
      }

      ~CurveAdapter()
      {
         Dispose();
      }

      public int? LegendIndex => Curve.LegendIndex;

      public ICurve Curve { get; }

      public void Dispose()
      {
         _dataTable.Dispose();
         foreach (var series in Series)
         {
            series.Dispose();
         }

         Curve.PropertyChanged -= onCurvePropertyChanged;
         _xAxis.PropertyChanged -= onAxisPropertyChanged;
         _yAxis.PropertyChanged -= onAxisPropertyChanged;
      }

      private void initializeData()
      {
         var yColumnNames = createData();
         refreshData();
         createAndBindSeries(yColumnNames);
      }

      private string getLegendText()
      {
         var legendText = !string.IsNullOrEmpty(Curve.Name) ? Curve.Name : Error.CurveNameMissing;
         if (_axisAdapters == null || _axisAdapters.Count(x => x.Visible) <= 2)
            return legendText;

         return $"({Curve.yAxisType}) {legendText}";
      }

      private IEnumerable<string> createData()
      {
         IList<string> yColumnNames = new List<string>();

         switch (dataMode)
         {
            case DataModes.Invalid:
               throw new InvalidArgumentException("Invalid RelatedColumns for " + Curve.yData.Name);

            case DataModes.SingleValue:
               yColumnNames.Add(Y);
               break;

            case DataModes.StdDevG:
               yColumnNames.Add(LOW);
               yColumnNames.Add(HIGH);
               yColumnNames.Add(Y);
               yColumnNames.Add(Y2);
               break;

            case DataModes.StdDevA:
               yColumnNames.Add(LOW);
               yColumnNames.Add(HIGH);
               yColumnNames.Add(Y);
               yColumnNames.Add(Y2);
               break;

            case DataModes.StdDevGPop:
               yColumnNames.Add(LOW);
               yColumnNames.Add(HIGH);
               yColumnNames.Add(Y);
               break;

            case DataModes.StdDevAPop:
               yColumnNames.Add(LOW);
               yColumnNames.Add(HIGH);
               yColumnNames.Add(Y);
               break;
         }

         _dataTable.AddColumn<float>(X);
         _dataTable.AddColumns<float>(yColumnNames);

         if (HasLLOQ)
            _dataTable.AddColumns<float>(LLOQ_SUFFIX);

         _dataTable.AddColumn<int>(INDEX_OF_VALUE_IN_CURVE);

         return yColumnNames;
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

      private void createAndBindSeries(IEnumerable<string> yColumnNames)
      {
         var series = createSeries(Curve.Id, ViewType.ScatterLine, yColumnNames.ToArray());

         if (dataMode == DataModes.StdDevA || dataMode == DataModes.StdDevG)
         {
            //add additional series for points and line and upper and lower markers
            series.ShowInLegend = Curve.VisibleInLegend;
            createPointLineSeries();
            createUpperPointSeries();
            createLowerPointSeries();
         }

         if (dataMode == DataModes.StdDevAPop || dataMode == DataModes.StdDevGPop)
         {
            createPointLineSeries();
         }

         if (HasLLOQ)
            createLowerLimitOfQuantificationSeries();
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
            LegendText = getLegendText(),
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

      private enum DataModes
      {
         Invalid,
         SingleValue,
         StdDevA,
         StdDevG,
         StdDevAPop,
         StdDevGPop
      }

      private DataModes dataMode
      {
         get
         {
            if (!Curve.yData.RelatedColumns.Any())
               return DataModes.SingleValue;

            if (Curve.yData.RelatedColumns.Count() > 1)
               return DataModes.Invalid;

            if (Curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticStdDev))
               return DataModes.StdDevA;

            if (Curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricStdDev))
               return DataModes.StdDevG;

            if (Curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
               return DataModes.StdDevAPop;

            if (Curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
               return DataModes.StdDevGPop;

            return DataModes.Invalid;
         }
      }

      /// <summary>
      ///    AxisAdapters are used for maintaining of this association as well,
      ///    but they are not available before the first series is added to the XtraCharts.ChartControl,
      ///    no AxisView objects are available and so the AxisAdapters.
      ///    Therefore they have to be attached afterwards, see ChartDisplayPresenter.AddCurve
      /// </summary>
      public void AttachAxisAdapters(ICache<AxisTypes, IAxisAdapter> axisAdapters)
      {
         _axisAdapters = axisAdapters;
         foreach (var series in _series)
         {
            series.LegendText = getLegendText();
            var xySeriesView = series.View as XYDiagramSeriesViewBase;
            if (xySeriesView != null)
               xySeriesView.AxisY = _axisAdapters[Curve.yAxisType].AxisView as AxisYBase;
         }
      }

      public IReadOnlyList<Series> Series => _series;

      public string Id => Curve.Id;

      public void RefreshLegendText()
      {
         foreach (var series in _series)
         {
            series.LegendText = getLegendText();
         }
      }

      public void Refresh()
      {
         refreshData();
         refreshView();
      }

      private void onAxisPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var axis = sender as IAxis;
         if (axis == null) return;

         if (!axisPropertyIsCoordinateSystemRelevant(e.PropertyName))
            return;

         Refresh();
      }

      private bool axisPropertyIsCoordinateSystemRelevant(string propertyName)
      {
         return (propertyName == Helpers.Property<IAxis>(a => a.UnitName).Name
                 || propertyName == Helpers.Property<IAxis>(a => a.Scaling).Name
                 || propertyName == Helpers.Property<IAxis>(a => a.NumberMode).Name);
      }

      private void onCurvePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var curve = sender as ICurve;
         if (curve == null) return;

         if (e.PropertyName == Helpers.Property<ICurve>(c => c.Name).Name)
         {
            foreach (var s in _series)
            {
               s.LegendText = getLegendText();
            }
         }
         else if (e.PropertyName == Helpers.Property<ICurve>(c => c.xData).Name
                  || e.PropertyName == Helpers.Property<ICurve>(c => c.yData).Name
                  || e.PropertyName == Helpers.Property<ICurve>(c => c.InterpolationMode).Name)
         {
            refreshData();
         }
         else if (e.PropertyName == Helpers.Property<ICurve>(c => c.yAxisType).Name)
         {
            _yAxis.PropertyChanged -= onAxisPropertyChanged;
            _yAxis = _axisAdapters[curve.yAxisType].Axis;
            _yAxis.PropertyChanged += onAxisPropertyChanged;

            foreach (var series in _series)
            {
               series.LegendText = getLegendText();
               var xySeriesView = series.View as XYDiagramSeriesViewBase;
               if (xySeriesView != null)
                  xySeriesView.AxisY = _axisAdapters[curve.yAxisType].AxisView as AxisYBase;
            }

            refreshData();
         }

         refreshView();
      }

      private void refreshView()
      {
         var curveOptions = Curve.CurveOptions;
         var series = _series[0];
         if (series == null) return;

         switch (dataMode)
         {
            case DataModes.Invalid:
               throw new InvalidArgumentException($"Invalid RelatedColumns for {Curve.yData.Name}");

            case DataModes.SingleValue:

               setSeriesOptionsForSingleValue(series);
               break;

            case DataModes.StdDevG:
            case DataModes.StdDevA:
               if (!seriesIsCandleStick(series))
               {
                  series.ChangeView(ViewType.CandleStick);
               }

               var financialSeriesView = series.View as FinancialSeriesViewBase;
               if (financialSeriesView != null)
               {
                  financialSeriesView.ReductionOptions.Visible = false;
                  financialSeriesView.LevelLineLength = 0;
                  financialSeriesView.LineThickness = 1;
                  financialSeriesView.Color = curveOptions.Color;
               }

               updateLinkedSeries(series);
               break;

            case DataModes.StdDevGPop:
            case DataModes.StdDevAPop:
               if (!seriesIsRangeArea(series))
               {
                  series.ChangeView(ViewType.RangeArea);
               }

               var rangeAreaSeriesView = series.View as RangeAreaSeriesView;
               if (rangeAreaSeriesView != null)
               {
                  rangeAreaSeriesView.Marker1Visibility = DefaultBoolean.False;
                  rangeAreaSeriesView.Marker2Visibility = DefaultBoolean.False;
                  rangeAreaSeriesView.FillStyle.FillMode = FillMode.Solid;
                  rangeAreaSeriesView.Color = curveOptions.Color;
                  rangeAreaSeriesView.Transparency = Constants.Population.STD_DEV_CURVE_TRANSPARENCY;
               }

               updateLinkedSeries(series);
               break;
         }

         setLowerLimitOfQuantificationOptions();
      }

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
            setSeriesOptionsForSingleValue(_series[1]);
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

      private bool seriesIs<TSeries>(Series series) where TSeries : SeriesViewBase => series.View.GetType() == typeof (TSeries);

      private void setSeriesOptionsForSingleValue(Series series)
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
         string dataErrorMessage = "";

         _dataTable.Clear();
         var mode = dataMode;
         if (mode == DataModes.Invalid)
            throw new InvalidArgumentException("Invalid RelatedColumns for " + Curve.yData.Name);

         if (!dimensionsConsistentToAxisUnits()) return; //show no values

         var xDimension = Curve.XDimension;
         var yDimension = Curve.YDimension;
         var xUnit = xDimension.Unit(_xAxis.UnitName);
         var yUnit = yDimension.Unit(_yAxis.UnitName);
         var xData = Curve.xData;
         var yData = activeYData();
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

               //float baseValue = rawBaseValue;
               var row = _dataTable.NewRow();
               DataColumn relatedColumn;
               double stdDev;
               row[X] = x;
               row[INDEX_OF_VALUE_IN_CURVE] = baseGrid.Values.ToList().IndexOf(baseValue);
               if (HasLLOQ)
                  row[LLOQ_SUFFIX] = LLOQ;
               row[Y] = y;

               switch (mode)
               {
                  case DataModes.SingleValue:
                     break;
                  case DataModes.StdDevA:
                     relatedColumn = yData.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);
                     stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, relatedColumn.GetValue(baseValue));
                     if (!isValidValue(stdDev))
                        stdDev = 0;

                     row[HIGH] = y + stdDev;
                     row[LOW] = y - stdDev;
                     row[Y2] = y;

                     if (!isValidYValue(y - stdDev))
                        row[LOW] = y;

                     break;
                  case DataModes.StdDevG:
                     relatedColumn = yData.GetRelatedColumn(AuxiliaryType.GeometricStdDev);
                     stdDev = relatedColumn.GetValue(baseValue);
                     if (!isValidValue(stdDev) || stdDev == 0)
                        stdDev = 1;

                     if (!isValidYValue(y / stdDev))
                        continue;

                     row[LOW] = y / stdDev;
                     row[HIGH] = y * stdDev;
                     row[Y2] = y;

                     break;
                  case DataModes.StdDevAPop:
                     stdDev = yDimension.BaseUnitValueToUnitValue(yUnit, Curve.yData.GetValue(baseValue));
                     if (!isValidValue(stdDev))
                        stdDev = 0;

                     if (!isValidYValue(y - stdDev))
                        continue;

                     row[LOW] = y - stdDev;
                     row[HIGH] = y + stdDev;

                     break;
                  case DataModes.StdDevGPop:
                     stdDev = Curve.yData.GetValue(baseValue);
                     if (!isValidValue(stdDev) || stdDev == 0)
                        stdDev = 1;

                     if (!isValidYValue(y / stdDev))
                        continue;

                     row[LOW] = y / stdDev;
                     row[HIGH] = y * stdDev;
                     break;
                  default:
                     throw new InvalidArgumentException("Invalid RelatedColumns for " + yData.Name);
               }

               _dataTable.Rows.Add(row);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception ex)
            {
               dataErrorMessage = ex.Message;
            }
         }

         foreach (var series in _series)
         {
            series.LegendText = getLegendText();

            if (!string.IsNullOrEmpty(dataErrorMessage))
               series.LegendText = $"{series.LegendText}\n{dataErrorMessage}";
         }

         if (_xAxis.NumberMode == NumberModes.Relative)
            setRelativeValues(X);

         if (_yAxis.NumberMode == NumberModes.Relative)
            setRelativeValues(Y);

         _dataTable.EndLoadData();
      }

      private BaseGrid activeBaseGrid(DataColumn xData, DataColumn yData)
      {
         if (Curve.CurveOptions.InterpolationMode == InterpolationModes.xLinear)
            return xData.BaseGrid;

         if (Curve.CurveOptions.InterpolationMode == InterpolationModes.yLinear)
            return yData.BaseGrid;

         throw new ArgumentException("InterpolationMode = " + Curve.InterpolationMode);
      }

      private DataColumn activeYData()
      {
         if (dataMode == DataModes.StdDevAPop)
            return Curve.yData.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop);

         if (dataMode == DataModes.StdDevGPop)
            return Curve.yData.GetRelatedColumn(AuxiliaryType.GeometricMeanPop);

         return Curve.yData;
      }

      private bool isValidXValue(double x)
      {
         return isValidAxisValue(x, _xAxis);
      }

      private bool isValidYValue(double y)
      {
         return isValidAxisValue(y, _yAxis);
      }

      private bool isValidAxisValue(double value, IAxis axis)
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
         var max = getMax(columnName);
         if (max <= 0) return;

         foreach (DataRow row in _dataTable.Rows)
         {
            if (row[columnName] != DBNull.Value)
               row[columnName] = (float) row[columnName] / max;

            if (columnName != Y)
               continue;

            switch (dataMode)
            {
               case DataModes.StdDevG:
               case DataModes.StdDevA:
                  if (row[Y2] != DBNull.Value)
                     row[Y2] = (float) row[Y2] / max;
                  if (row[LOW] != DBNull.Value)
                     row[LOW] = (float) row[LOW] / max;
                  if (row[HIGH] != DBNull.Value)
                     row[HIGH] = (float) row[HIGH] / max;
                  break;
               case DataModes.StdDevGPop:
               case DataModes.StdDevAPop:
                  if (row[HIGH] != DBNull.Value)
                     row[HIGH] = (float) row[HIGH] / max;
                  break;
            }
         }
      }

      private float getMax(string columnName)
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
         if (columnName == Y && maxRow != null)
         {
            if (dataMode == DataModes.StdDevAPop)
               max = ((float) maxRow[LOW] + (float) maxRow[HIGH]) / 2;
            if (dataMode == DataModes.StdDevGPop)
               max = (float) Math.Sqrt(((float) maxRow[LOW]) * ((float) maxRow[HIGH]));
         }
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
         Series.Each(s => s.Visible = visible);
      }

      public void ShowCurveInLegend(bool visibleInLegend)
      {
         if (isCandleStickView())
            setLegendVisibilityForCandleStickView(visibleInLegend);
         else
            _series[0].ShowInLegend = visibleInLegend;

         refreshView();
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
   }
}