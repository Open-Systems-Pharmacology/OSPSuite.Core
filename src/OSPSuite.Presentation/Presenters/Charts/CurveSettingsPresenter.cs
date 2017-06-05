using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public enum CurveOptionsColumns
   {
      Id,
      Name,
      xData,
      yData,
      InterpolationMode,
      yAxisType,
      Visible,
      VisibleInLegend,
      Color,
      LineStyle,
      Symbol,
      LineThickness,
      ShowLowerLimitOfQuantification
   }

   public interface ICurveSettingsPresenter : IPresenter<ICurveSettingsView>, IPresenterWithColumnSettings
   {
      void Edit(CurveChart chart);
      void Clear();
      Func<DataColumn, string> CurveNameDefinition { set; get; }

      void Remove(CurveDTO curve);
      event Action<Curve> RemoveCurve;

      void AddCurvesForColumns(IReadOnlyList<DataColumn> dataColumns);
      event Action<IReadOnlyList<DataColumn>> AddCurves;

      void SetCurveXData(CurveDTO curve, DataColumn dataColumn);
      void SetCurveYData(CurveDTO curve, DataColumn dataColumn);

      IEnumerable<AxisTypes> AllYAxisTypes { get; }
      string ToolTipFor(CurveDTO curveDTO);
      void MoveSeriesInLegend(CurveDTO curveBeingMoved, CurveDTO targetCurve);
      void Refresh();

      void NotifyCurvePropertyChange(CurveDTO curveDTO);
      event Action<Curve> CurvePropertyChanged;         
   }

   internal class CurveSettingsPresenter : PresenterWithColumnSettings<ICurveSettingsView, ICurveSettingsPresenter>, ICurveSettingsPresenter
   {
      private readonly IDimensionFactory _dimensionFactory;

      public event Action<Curve> RemoveCurve = delegate { };
      public event Action<IReadOnlyList<DataColumn>> AddCurves = delegate { };

      public Func<DataColumn, string> CurveNameDefinition { get; set; }
      private CurveChart _chart;
      private readonly List<CurveDTO> _allCurvesDTOs = new List<CurveDTO>();

      public CurveSettingsPresenter(ICurveSettingsView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
         CurveNameDefinition = column => column.Name;
      }

      public void MoveSeriesInLegend(CurveDTO curveBeingMoved, CurveDTO targetCurve)
      {
         _chart.MoveSeriesInLegend(curveBeingMoved.Curve, targetCurve.Curve);
      }

      public string ToolTipFor(CurveDTO curveDTO)
      {
         if (CurveNameDefinition == null || curveDTO.yData == null)
            return string.Empty;

         return CurveNameDefinition(curveDTO.yData);
      }

      protected override void SetDefaultColumnSettings()
      {
         AddColumnSettings(CurveOptionsColumns.Id).WithCaption(Captions.Chart.CurveOptions.Id).WithVisible(false);
         AddColumnSettings(CurveOptionsColumns.Name).WithCaption(Captions.Chart.CurveOptions.Name);
         AddColumnSettings(CurveOptionsColumns.xData).WithCaption(Captions.Chart.CurveOptions.xData);
         AddColumnSettings(CurveOptionsColumns.yData).WithCaption(Captions.Chart.CurveOptions.yData);
         AddColumnSettings(CurveOptionsColumns.yAxisType).WithCaption(Captions.Chart.CurveOptions.yAxisType);
         AddColumnSettings(CurveOptionsColumns.InterpolationMode).WithCaption(Captions.Chart.CurveOptions.InterpolationMode);
         AddColumnSettings(CurveOptionsColumns.Color).WithCaption(Captions.Chart.CurveOptions.Color);
         AddColumnSettings(CurveOptionsColumns.LineStyle).WithCaption(Captions.Chart.CurveOptions.LineStyle);
         AddColumnSettings(CurveOptionsColumns.Symbol).WithCaption(Captions.Chart.CurveOptions.Symbol);
         AddColumnSettings(CurveOptionsColumns.LineThickness).WithCaption(Captions.Chart.CurveOptions.LineThickness).WithVisible(false);
         AddColumnSettings(CurveOptionsColumns.Visible).WithCaption(Captions.Chart.CurveOptions.Visible);
         AddColumnSettings(CurveOptionsColumns.VisibleInLegend).WithCaption(Captions.Chart.CurveOptions.VisibleInLegend);
         AddColumnSettings(CurveOptionsColumns.ShowLowerLimitOfQuantification).WithCaption(Captions.LLOQ).WithVisible(false);
      }

      public void Edit(CurveChart chart)
      {
         _chart = chart;
         _allCurvesDTOs.Clear();
         _chart?.Curves.Each(addCurve);
         rebind();
      }

      public void Clear()
      {
         Edit(null);
      }

      public void Remove(CurveDTO curveDTO)
      {
         RemoveCurve(curveDTO.Curve);
      }

      public void AddCurvesForColumns(IReadOnlyList<DataColumn> dataColumns)
      {
         AddCurves(dataColumns);
      }

      private CurveDTO mapFrom(Curve curve)
      {
         return new CurveDTO(curve, _chart);
      }

      public void Refresh()
      {
         foreach (var curve in _chart.Curves)
         {
            if (hasCurveDTOFor(curve))
               continue;

            addCurve(curve);
         }

         foreach (var curveDTO in _allCurvesDTOs.ToList())
         {
            if (!_chart.HasCurve(curveDTO.Id))
               _allCurvesDTOs.Remove(curveDTO);
         }

         rebind();
      }

      public void NotifyCurvePropertyChange(CurveDTO curveDTO)
      {
         CurvePropertyChanged(curveDTO.Curve);
      }

      public event Action<Curve> CurvePropertyChanged = delegate { };

      private void rebind()
      {
         View.BindToSource(_allCurvesDTOs);
      }

      private void addCurve(Curve curve)
      {
         _allCurvesDTOs.Add(mapFrom(curve));
      }

      private bool hasCurveDTOFor(Curve curve)
      {
         return _allCurvesDTOs.Any(x => Equals(x.Curve, curve));
      }

      private void onCurvesItemChanged(object sender, ItemChangedEventArgs e)
      {
         var curve = e.Item as Curve;
         if (curve == null) return;

         if (propertyIsYAxisType(e.PropertyName) || propertyIsYDimension(e.PropertyName))
            syncUnitsAndCheckDimension(curve, _chart.AxisBy(curve.yAxisType));

         else if (propertyIsXDimension(e.PropertyName))
            syncUnitsAndCheckDimension(curve, _chart.AxisBy(AxisTypes.X));

         View.RefreshData();
      }

      private void onAxesItemChanged(object sender, ItemChangedEventArgs e)
      {
         var axis = e.Item as Axis;
         if (axis == null) return;

         if (!propertyIsDimension(e.PropertyName) && !propertyIsUnit(e.PropertyName))
            return;

         foreach (var curve in _chart.Curves)
         {
            syncUnitsAndCheckDimension(curve, axis);
         }

         View.RefreshData();
      }

      private bool propertyIsXDimension(string propertyName)
      {
         return false;
      }

      private bool propertyIsYDimension(string propertyName)
      {
         return false;
      }

      private bool propertyIsYAxisType(string propertyName)
      {
         return false;
      }

      private bool propertyIsUnit(string propertyName)
      {
         return false;
      }

      private bool propertyIsDimension(string propertyName)
      {
         return false;
      }

      public void CurveAdded(Curve curve)
      {
         syncUnitsAndCheckDimension(curve, _chart.AxisBy(AxisTypes.X));
         syncUnitsAndCheckDimension(curve, _chart.AxisBy(curve.yAxisType));
      }

      public IEnumerable<AxisTypes> AllYAxisTypes => _chart.AllUsedYAxisTypes;

      public void SetCurveXData(CurveDTO curve, DataColumn dataColumn)
      {
         _chart.SetxData(curve.Curve, dataColumn, _dimensionFactory);
      }

      public void SetCurveYData(CurveDTO curve, DataColumn dataColumn)
      {
         _chart.SetyData(curve.Curve, dataColumn, _dimensionFactory);
      }

      private void syncUnitsAndCheckDimension(Curve curve, Axis axis)
      {
         if (axis == null)
            return;

         updateDisplayUnitInCurveData(curve, axis);
      }

      private void updateDisplayUnitInCurveData(Curve curve, Axis axis)
      {
         DataColumn data;
         if (axis.AxisType == AxisTypes.X)
            data = curve.xData;
         else if (axis.AxisType == curve.yAxisType)
            data = curve.yData;
         else
            return;

         data.DisplayUnit = axis.Unit;
      }
   }
}