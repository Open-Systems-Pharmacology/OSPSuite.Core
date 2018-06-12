using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
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
      private readonly Axis _xAxis;
      private readonly Axis _yAxis;

      private readonly DataTable _dataTable;
      protected readonly List<Series> _series;
      private readonly AxisTypes _yAxisType;
      private string _LLOQSeriesId;

      private const string X = "X";
      protected const string Y = "Y";
      private const string LLOQ_SUFFIX = "LLOQ";
      private const string INDEX_OF_VALUE_IN_CURVE = "INDEX_OF_VALUE_IN_CURVE";
      public Curve Curve { get; }

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
         _LLOQSeriesId = string.Empty;
         initializeData();
      }

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

            return Curve.yDimension.BaseUnitValueToUnitValue(Curve.yDimension.Unit(_yAxis.UnitName), Convert.ToDouble(Curve.yData.DataInfo.LLOQ));
         }
      }

      private void createAndBindSeries()
      {
         CreateSeries();

         if (HasLLOQ)
            createLLOQSeries();

         attachYAxisToSeries();

         _chartControl.Series.AddRange(_series.ToArray());
      }

      protected abstract void CreateSeries();

      protected Series CreatePointLineSeries()
      {
         var series = CreateScatterLineSeries($"{Curve.Id}_Line");
         series.ShowInLegend = true;
         series.Visible = Curve.CurveOptions.IsReallyVisible;
         return series;
      }

      protected Series CreateScatterLineSeries(string id)
      {
         return CreateSeries<ScatterLineSeriesView>(id, ViewType.ScatterLine, Y, scatterLineSeriesView => { scatterLineSeriesView.EnableAntialiasing = DefaultBoolean.True; });
      }

      private void createLLOQSeries()
      {
         _LLOQSeriesId = $"{Curve.Id}_{LLOQ_SUFFIX}";
         var series = CreateSeries<LineSeriesView>(_LLOQSeriesId, ViewType.Line, LLOQ_SUFFIX, lineSeriesView =>
         {
            lineSeriesView.LineStyle.DashStyle = DashStyle.Dot;
            lineSeriesView.LineStyle.Thickness = 2;
            lineSeriesView.MarkerVisibility = DefaultBoolean.False;
         });
         series.Visible = Curve.ShowLLOQ && Curve.Visible;
      }

      protected Series CreateSeries(string name, ViewType viewType, string valueDataMember)
      {
         return CreateSeries<SeriesViewBase>(name, viewType, valueDataMember);
      }

      protected Series CreateSeries(string name, ViewType viewType, IReadOnlyList<string> valueDataMembers)
      {
         return CreateSeries<SeriesViewBase>(name, viewType, valueDataMembers);
      }

      protected Series CreateSeries<TSeriesView>(string name, ViewType viewType, string valueDataMember, Action<TSeriesView> configuration = null) where TSeriesView : SeriesViewBase
      {
         return CreateSeries(name, viewType, new[] {valueDataMember}, configuration);
      }

      protected Series CreateSeries<TSeriesView>(string name, ViewType viewType, IReadOnlyList<string> valueDataMembers, Action<TSeriesView> configuration = null) where TSeriesView : SeriesViewBase
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

         series.ValueDataMembers.AddRange(valueDataMembers.ToArray());

         var view = series.View as TSeriesView;
         if (configuration != null && view != null)
         {
            configuration(view);
         }

         _series.Add(series);

         return series;
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

      private void refreshView()
      {
         RefreshSeries();
         updateSeriesCaption();
         setLLOQOptions();
         ShowCurveInLegend(Curve.VisibleInLegend);
      }

      private void updateSeriesCaption()
      {
         var legendText = Curve.Name;
         if (string.IsNullOrEmpty(legendText))
            legendText = Warning.CurveNameIsMissing;

         _series.Each(s => s.LegendText = legendText);
      }

      protected abstract void RefreshSeries();

      protected void UpdateRelatedSeriesOf(Series mainSeries)
      {
         updateSeriesVisibility(visible: Curve.Visible);

         foreach (var s in _series.Except(new[] {mainSeries}))
         {
            var xySeriesView = s.View as XYDiagramSeriesViewBase;
            if (xySeriesView != null)
               xySeriesView.Color = Curve.Color;
         }
      }

      private void setLLOQOptions()
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

   
      private bool seriesIsScatterLine(Series series) => seriesIs<ScatterLineSeriesView>(series);
      private bool seriesIsPoint(Series series) => seriesIs<PointSeriesView>(series);

      private bool seriesIs<TSeries>(Series series) where TSeries : SeriesViewBase => series.View.GetType() == typeof(TSeries);

      protected void UpdateLineSeries(Series series)
      {
         if (Curve.LineStyle == LineStyles.None & seriesIsScatterLine(series))
            series.ChangeView(ViewType.Point);

         else if (Curve.LineStyle != LineStyles.None & seriesIsPoint(series))
            series.ChangeView(ViewType.ScatterLine);

         series.Visible = Curve.IsReallyVisible && dimensionsConsistentToAxisUnits();

         var xySeriesView = series.View as XYDiagramSeriesViewBase;
         if (xySeriesView != null)
            xySeriesView.Color = Curve.Color;

         var pointSeriesView = series.View as PointSeriesView;
         if (pointSeriesView != null)
         {
            pointSeriesView.PointMarkerOptions.Kind = mapFrom(Curve.Symbol);
            updateMarker(pointSeriesView.PointMarkerOptions);
         }

         if (seriesIsScatterLine(series))
         {
            var lineSeriesView = series.View as LineSeriesView;
            if (lineSeriesView != null)
            {
               if (Curve.LineStyle != LineStyles.None)
                  lineSeriesView.LineStyle.DashStyle = mapFrom(Curve.LineStyle);

               lineSeriesView.LineStyle.Thickness = Curve.LineThickness;
               lineSeriesView.MarkerVisibility = (Curve.Symbol != Symbols.None)
                  ? DefaultBoolean.True
                  : DefaultBoolean.False;

               updateMarker(lineSeriesView.LineMarkerOptions);
            }
         }
      }

      private void updateMarker(SimpleMarker marker)
      {
         marker.FillStyle.FillMode = FillMode.Solid;
         marker.Size = UIConstants.Chart.SERIES_MARKER_SIZE + Curve.LineThickness * Curve.LineThickness;
      }

      private void refreshData()
      {
         _dataTable.Clear();

         //show no values
         if (!dimensionsConsistentToAxisUnits())
            return;

         var xDimension = Curve.xDimension;
         var yDimension = Curve.yDimension;
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

               if (!isValidXValue(x) || !IsValidYValue(y))
                  continue;

               var row = _dataTable.NewRow();
               row[X] = x;
               row[Y] = y;
               row[INDEX_OF_VALUE_IN_CURVE] = baseGrid.IndexOf(baseValue);

               if (HasLLOQ)
                  row[LLOQ_SUFFIX] = LLOQ;

               AddRelatedValuesToRow(row, yData, yDimension, yUnit, y, baseValue);

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

      protected abstract bool AddRelatedValuesToRow(DataRow row, DataColumn yData, IDimension yDimension, Unit yUnit, double y, float baseValue);

      private BaseGrid activeBaseGrid(DataColumn xData, DataColumn yData)
      {
         if (Curve.InterpolationMode == InterpolationModes.xLinear)
            return xData.BaseGrid;

         if (Curve.InterpolationMode == InterpolationModes.yLinear)
            return yData.BaseGrid;

         throw new ArgumentException("InterpolationMode = " + Curve.InterpolationMode);
      }

      protected abstract DataColumn ActiveYData { get; }

      private bool isValidXValue(double x)
      {
         return isValidAxisValue(x, _xAxis);
      }

      protected bool IsValidYValue(double y)
      {
         return isValidAxisValue(y, _yAxis);
      }

      private bool isValidAxisValue(double value, Axis axis)
      {
         if (!IsValidValue(value))
            return false;

         return axis.Scaling == Scalings.Linear || value > 0;
      }

      protected static bool IsValidValue(double value)
      {
         return !double.IsInfinity(value) && !double.IsNaN(value);
      }

      private void setRelativeValues(string columnName)
      {
         var max = getMax(columnName);
         if (max <= 0) return;

         foreach (DataRow row in _dataTable.Rows)
         {
            ScaleValue(row, max, columnName);

            if (columnName != Y)
               continue;

            SetRelatedRelativeValuesForRow(row, max);
         }
      }

      protected void ScaleValue(DataRow row, float max, string value)
      {
         if (row[value] != DBNull.Value)
            row[value] = (float) row[value] / max;
      }

      protected abstract void SetRelatedRelativeValuesForRow(DataRow row, float max);

      private float getMax(string columnName)
      {
         float max = 0;
         DataRow maxRow = null;
         foreach (DataRow row in _dataTable.Rows)
         {
            if (row[columnName] == DBNull.Value)
               continue;

            var value = (float) row[columnName];
            if (!float.IsNaN(value) && value > max)
            {
               max = value;
               maxRow = row;
            }
         }

         return GetRelatedMax(columnName, maxRow, max);
      }

      protected virtual float GetRelatedMax(string columnName, DataRow maxRow, float max)
      {
         //default implementations returns max
         return max;
      }

      private bool dimensionsConsistentToAxisUnits()
      {
         return Curve.xDimension.CanConvertToUnit(_xAxis.UnitName) &&
                Curve.yDimension.CanConvertToUnit(_yAxis.UnitName);
      }

      public void ShowAllSeries()
      {
         updateSeriesVisibility(visible: true);
      }

      private void updateSeriesVisibility(bool visible)
      {
         _series.Each(s => s.Visible = visible);
      }

      public abstract void ShowCurveInLegend(bool showInLegend);

      public bool ContainsSeries(string seriesId)
      {
         return _series.Any(s => string.Equals(s.Name, seriesId));
      }

      public bool IsSeriesLLOQ(string seriesId)
      {
         return string.Equals(_LLOQSeriesId, seriesId);
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
   }
}