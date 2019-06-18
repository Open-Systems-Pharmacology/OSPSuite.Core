using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Chart
{
   public class CurveChart : Notifier, IWithId, IChart, IVisitable<IVisitor>, IWithAxes
   {
      public ChartFontAndSizeSettings FontAndSize { get; } = new ChartFontAndSizeSettings();
      public ChartSettings ChartSettings { get; } = new ChartSettings();
      private readonly Cache<string, Curve> _curves;
      private readonly Cache<AxisTypes, Axis> _axes;
      public string Id { get; set; }
      private bool _previewSettings;
      private bool _includeOriginData;
      private string _description;
      private string _name;
      private string _title;
      private string _originText;
      public Scalings DefaultYAxisScaling { get; set; }

      public CurveChart()
      {
         Id = string.Empty;
         _name = string.Empty;
         _description = string.Empty;
         _title = string.Empty;
         _axes = new Cache<AxisTypes, Axis>(axis => axis.AxisType, x => null);
         _curves = new Cache<string, Curve>(curve => curve.Id, x => null);
         DefaultYAxisScaling = Scalings.Log;
      }

      public void CopyChartSettingsFrom(IChartManagement chart)
      {
         ChartSettings.UpdatePropertiesFrom(chart.ChartSettings);
      }

      public void Clear()
      {
         _axes.Clear();
         _curves.Clear();
      }

      public void MoveCurvesInLegend(Curve movingCurve, Curve targetCurve)
      {
         var index = targetCurve.LegendIndex;
         Curves.Where(curve => curve.LegendIndex >= index).Each(curve => curve.LegendIndex++);
         movingCurve.LegendIndex = index;
      }

      public IReadOnlyCollection<Axis> Axes => _axes;

      public bool HasAxis(AxisTypes axisType) => _axes.Contains(axisType);

      public void AddAxis(Axis axis) => _axes.Add(axis);

      public void RemoveAxis(Axis axis)
      {
         _axes.Remove(axis.AxisType);
         var curvesUsingAxis = Curves.Where(x => x.yAxisType == axis.AxisType).ToList();
         curvesUsingAxis.Each(x => x.yAxisType = AxisTypes.Y);
      }

      public Axis AddNewAxis()
      {
         return (from axisType in EnumHelper.AllValuesFor<AxisTypes>()
            where !_axes.Contains(axisType)
            select AddNewAxisFor(axisType)).FirstOrDefault();
      }

      public Axis AxisBy(AxisTypes axisTypes) => _axes[axisTypes];

      public Axis XAxis => AxisBy(AxisTypes.X);

      public Axis YAxisFor(Curve curve) => AxisBy(curve.yAxisType);

      public IReadOnlyCollection<Curve> Curves => _curves;

      public IEnumerable<Curve> AllCurvesOnYAxis(AxisTypes yAxisType) => Curves.Where(x => x.yAxisType == yAxisType);

      public IEnumerable<Curve> AllVisibleCurvesOnYAxis(AxisTypes yAxisType) => AllCurvesOnYAxis(yAxisType).Where(x => x.Visible);

      public IEnumerable<AxisTypes> AllUsedAxisTypes => _axes.Select(x => x.AxisType);

      public IEnumerable<AxisTypes> AllUsedYAxisTypes => AllUsedAxisTypes.Except(new[] { AxisTypes.X });

      public IEnumerable<Axis> AllUsedYAxis => _axes.Where(x => x.AxisType != AxisTypes.X);

      public IEnumerable<AxisTypes> AllUsedSecondaryAxisTypes => AllUsedYAxisTypes.Except(new[] { AxisTypes.Y });

      public bool HasCurve(string curveId) => _curves.Contains(curveId);

      public Curve CurveBy(string curveId) => _curves[curveId];

      public void RemoveCurve(Curve curve)
      {
         RemoveCurve(curve?.Id);
      }

      public void RemoveCurve(string curveId)
      {
         if (string.IsNullOrEmpty(curveId))
            return;

         if (!_curves.Contains(curveId))
            return;

         var yAxisType = _curves[curveId].yAxisType;
         _curves.Remove(curveId);
         updateAxesForRemovedCurve(yAxisType);
      }

      public Curve CreateCurve(DataColumn columnX, DataColumn columnY, string curveName, IDimensionFactory dimensionFactory)
      {
         var curve = FindCurveWithSameData(columnX, columnY);
         if (curve != null)
            return curve;

         curve = new Curve {Name = curveName};
         curve.SetxData(columnX, dimensionFactory);
         curve.SetyData(columnY, dimensionFactory);
         return curve;
      }

      public void SetxData(Curve curve, DataColumn column, IDimensionFactory dimensionFactory)
      {
         curve.SetxData(column, dimensionFactory);
         updateAxesForAddedCurve(curve);
      }

      public void SetyData(Curve curve, DataColumn column, IDimensionFactory dimensionFactory)
      {
         curve.SetyData(column, dimensionFactory);
         updateAxesForAddedCurve(curve);
      }

      public Curve FindCurveWithSameData(DataColumn xData, DataColumn yData)
      {
         return Curves.FirstOrDefault(curve => xData.Id == curve.xData.Id && yData.Id == curve.yData.Id);
      }

      public void AddCurve(Curve curve, bool useAxisDefault = true)
      {
         if (curve?.xData == null || curve.yData == null)
            return;

         if (_curves.Contains(curve.Id))
            return;

         updateAxesForAddedCurve(curve);

         if (useAxisDefault)
            copyLineStyleAndColorFromYAxisDefault(curve);

         _curves.Add(curve);

         if (!curve.LegendIndex.HasValue)
            curve.LegendIndex = nextLegendIndex();
      }

      private int? nextLegendIndex()
      {
         var allCurvesWithValidLegendIndex = Curves.Where(x => x.LegendIndex.HasValue).ToList();
         if (!allCurvesWithValidLegendIndex.Any())
            return 1;

         return allCurvesWithValidLegendIndex.Max(x => x.LegendIndex) + 1;
      }

      private void copyLineStyleAndColorFromYAxisDefault(Curve curve)
      {
         var axis = YAxisFor(curve);

         if (axis.DefaultLineStyle != LineStyles.None && curve.Symbol == Symbols.None)
            curve.LineStyle = axis.DefaultLineStyle;

         if (!isWhite(axis.DefaultColor))
            curve.Color = axis.DefaultColor;
      }

      private bool isWhite(Color color)
      {
         return color.R == Color.White.R && color.G == Color.White.G && color.B == Color.White.B;
      }

      public void AddCurveIfConsistent(Curve curve)
      {
         if (curve?.xData == null || curve.yData == null) return;
         AddCurve(curve, useAxisDefault: false);
      }

      private void updateAxesForAddedCurve(Curve curve)
      {
         updateAxis(AxisTypes.X, curve.xDimension, curve.xData.DisplayUnit);

         var axisTypeY = curve.yAxisType;
         var axisTypeYother = axisTypeY == AxisTypes.Y ? AxisTypes.Y2 : AxisTypes.Y;

         var yAxis = updateAxis(axisTypeY, curve.yDimension, curve.yData.DisplayUnit);

         if (curve.yDimension.HasSharedUnitNamesWith(yAxis.Dimension))
            return;

         var y2Axis = updateAxis(axisTypeYother, curve.yDimension, curve.yData.DisplayUnit);

         if (curve.yDimension.HasSharedUnitNamesWith(y2Axis.Dimension))
            curve.yAxisType = axisTypeYother;
      }

      private Axis updateAxis(AxisTypes axisType, IDimension dimension, Unit unit)
      {
         if (!_axes.Contains(axisType))
            AddNewAxisFor(axisType);

         var axis = _axes[axisType];
         if (axis.Dimension == null)
         {
            axis.Dimension = dimension;
            axis.UnitName = unit.Name;
         }

         return axis;
      }

      public Axis AddNewAxisFor(AxisTypes axisType)
      {
         var newAxis = new Axis(axisType);
         _axes.Add(newAxis);

         if (newAxis.IsYAxis)
            newAxis.Scaling = DefaultYAxisScaling;

         return newAxis;
      }

      private void updateAxesForRemovedCurve(AxisTypes yAxisType)
      {
         if (!Curves.Any())
            _axes[AxisTypes.X].Reset();

         if (Curves.All(c => c.yAxisType != yAxisType))
            _axes[yAxisType].Reset();
      }

      public void RemoveDatalessCurves()
      {
         removeCurves(curve => !curve.xData.IsInRepository() || !curve.yData.IsInRepository());
      }

      public void RemoveCurvesForColumn(DataColumn dataColumn)
      {
         removeCurves(curve => curve.xData == dataColumn || curve.yData == dataColumn);
      }

      public void RemoveCurvesForColumns(IEnumerable<DataColumn> dataColumns) => dataColumns.Each(RemoveCurvesForColumn);

      public void RemoveCurvesForDataRepository(DataRepository dataRepository) => RemoveCurvesForColumns(dataRepository);

      private void removeCurves(Func<Curve, bool> shouldRemoveCurveFunc)
      {
         var curvesToRemove = Curves.Where(shouldRemoveCurveFunc).ToList();
         curvesToRemove.Each(x => RemoveCurve(x.Id));
      }

      public void SynchronizeDataDisplayUnit()
      {
         _curves.Each(synchronizeDataDisplayUnitForCurve);
      }

      private void synchronizeDataDisplayUnitForCurve(Curve curve)
      {
         updateDisplayUnitInCurveData(curve.xData, XAxis);
         updateDisplayUnitInCurveData(curve.yData, YAxisFor(curve));
      }

      private void updateDisplayUnitInCurveData(DataColumn data, Axis axis)
      {
         if (axis == null || data == null)
            return;

         data.DisplayUnit = axis.Unit;
      }

      public IReadOnlyList<DataColumn> UsedColumns
      {
         get
         {
            var userColumns = new HashSet<DataColumn>();
            foreach (var curve in _curves)
            {
               if (curve.xData != null)
                  userColumns.Add(curve.xData);

               if (curve.yData != null)
                  userColumns.Add(curve.yData);
            }
            return userColumns.ToList();
         }
      }

       public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }

      public virtual string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
      }

      public virtual string Description
      {
         get => _description;
         set => SetProperty(ref _description, value);
      }

      public string Title
      {
         get => _title;
         set => SetProperty(ref _title, value);
      }

      public string OriginText
      {
         get => _originText;
         set => SetProperty(ref _originText, value);
      }

      public bool IncludeOriginData
      {
         get => _includeOriginData;
         set => SetProperty(ref _includeOriginData, value);
      }

      public bool PreviewSettings
      {
         get => _previewSettings;
         set => SetProperty(ref _previewSettings, value);
      }
   }
}