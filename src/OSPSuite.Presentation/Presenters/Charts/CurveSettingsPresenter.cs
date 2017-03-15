using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views.Charts;

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
      void SetDatasource(ICurveChart chart);
      Func<DataColumn, string> CurveNameDefinition { set; get; }

      void RemoveCurve(string curveId);

      /// <summary>
      ///    Adds a new curve to a the chart for the <paramref name="columnId" /> if one does not already exist.
      ///    Curve options will be applied if they are specified and a new curve is created.
      /// </summary>
      /// <returns>
      ///    The new curve or existing if chart already contains a curve for the specified <paramref name="columnId" />
      /// </returns>
      ICurve AddCurveForColumn(string columnId, CurveOptions defaultCurveOptions = null);

      void SetCurveXData(ICurve curve, string columnId);
      void SetCurveYData(ICurve curve, string columnId);

      IEnumerable<AxisTypes> GetAxisTypes();
      string ToolTipFor(ICurve curve);
      ICache<string, DataColumn> DataColumns { get; set; }
      void MoveSeriesInLegend(ICurve curveBeingMoved, ICurve targetCurve);
   }

   internal class CurveSettingsPresenter : PresenterWithColumnSettings<ICurveSettingsView, ICurveSettingsPresenter>, ICurveSettingsPresenter
   {
      public ICache<string, DataColumn> DataColumns { get; set; }
      private readonly IDimensionFactory _dimensionFactory;
      public Func<DataColumn, string> CurveNameDefinition { get; set; }
      private ICurveChart _chart;
      private readonly string _unitPropertyName;
      private readonly string _dimensionPropertyName;
      private readonly string _xDimensionPropertyName;
      private readonly string _yDimensionPropertyName;
      private readonly string _yAxisTypePropertyName;

      public CurveSettingsPresenter(ICurveSettingsView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
         CurveNameDefinition = mcDataColumn => mcDataColumn.Name;
         DataColumns = new Cache<string, DataColumn>(x => x.Id);
         _unitPropertyName = Helpers.Property<IAxis>(x => x.UnitName).Name;
         _dimensionPropertyName = Helpers.Property<IAxis>(x => x.Dimension).Name;
         _xDimensionPropertyName = Helpers.Property<ICurve>(c => c.XDimension).Name;
         _yDimensionPropertyName = Helpers.Property<ICurve>(c => c.YDimension).Name;
         _yAxisTypePropertyName = Helpers.Property<ICurve>(c => c.yAxisType).Name;
      }

      public void MoveSeriesInLegend(ICurve curveBeingMoved, ICurve targetCurve)
      {
         _chart.MoveSeriesInLegend(curveBeingMoved, targetCurve);
      }

      public string ToolTipFor(ICurve curve)
      {
         if (CurveNameDefinition == null || curve.yData == null)
            return string.Empty;

         return CurveNameDefinition(curve.yData);
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
         AddColumnSettings(CurveOptionsColumns.ShowLowerLimitOfQuantification).WithCaption(Captions.LLOQ);
      }

      public void SetDatasource(ICurveChart chart)
      {
         if (_chart != null)
         {
            _chart.Curves.ItemPropertyChanged -= onCurvesItemChanged;
            _chart.Curves.CollectionChanged -= onCurvesCollectionChanged;
            _chart.Axes.ItemPropertyChanged -= onAxesItemChanged;
            _chart.Axes.CollectionChanged -= onAxesCollectionChanged;
         }

         _chart = chart;

         if (_chart != null)
         {
            _chart.Curves.ItemPropertyChanged += onCurvesItemChanged;
            _chart.Curves.CollectionChanged += onCurvesCollectionChanged;

            View.BindToSource(_chart.Curves);

            _chart.Axes.ItemPropertyChanged += onAxesItemChanged;
            _chart.Axes.CollectionChanged += onAxesCollectionChanged;
         }
      }

      private void onCurvesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.Action != NotifyCollectionChangedAction.Add) return;

         foreach (var item in e.NewItems)
         {
            var curve = item as ICurve;
            if (curve == null) continue;
            CurveAdded(curve);
         }
      }

      private void onAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         //set yAxisType to y, if yAxis with current yAxisType of a curve is removed
         if (e.Action != NotifyCollectionChangedAction.Remove) return;

         foreach (var item in e.OldItems)
         {
            var axis = item as IAxis;
            if (axis == null) continue;
            foreach (var curve in _chart.Curves.Where(curve => curve.yAxisType == axis.AxisType))
            {
               curve.yAxisType = AxisTypes.Y;
            }
         }
      }

      private void onCurvesItemChanged(object sender, ItemChangedEventArgs e)
      {
         var curve = e.Item as ICurve;
         if (curve == null) return;

         if (propertyIsYAxisType(e.PropertyName) || propertyIsYDimension(e.PropertyName))
            syncUnitsAndCheckDimension(curve, _chart.Axes[curve.yAxisType]);

         else if (propertyIsXDimension(e.PropertyName))
            syncUnitsAndCheckDimension(curve, _chart.Axes[AxisTypes.X]);

         View.RefreshData();
      }

      private void onAxesItemChanged(object sender, ItemChangedEventArgs e)
      {
         var axis = e.Item as IAxis;
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
         return string.Equals(propertyName, _xDimensionPropertyName);
      }

      private bool propertyIsYDimension(string propertyName)
      {
         return string.Equals(propertyName, _yDimensionPropertyName);
      }

      private bool propertyIsYAxisType(string propertyName)
      {
         return string.Equals(propertyName, _yAxisTypePropertyName);
      }

      private bool propertyIsUnit(string propertyName)
      {
         return string.Equals(propertyName, _unitPropertyName);
      }

      private bool propertyIsDimension(string propertyName)
      {
         return string.Equals(propertyName, _dimensionPropertyName);
      }

      public void CurveAdded(ICurve curve)
      {
         if (_chart.Axes.Contains(AxisTypes.X))
            syncUnitsAndCheckDimension(curve, _chart.Axes[AxisTypes.X]);

         if (_chart.Axes.Contains(curve.yAxisType))
            syncUnitsAndCheckDimension(curve, _chart.Axes[curve.yAxisType]);
      }

      public void RemoveCurve(string curveId)
      {
         _chart.RemoveCurve(curveId);
      }

      public ICurve AddCurveForColumn(string columnId, CurveOptions defaultCurveOptions = null)
      {
         var dataColumn = DataColumns[columnId];
         var curve = _chart.CreateCurve(dataColumn.BaseGrid, dataColumn, CurveNameDefinition(dataColumn), _dimensionFactory);

         // Settings already in chart, make no changes
         if (_chart.Curves.Contains(curve.Id))
            return _chart.Curves[curve.Id];

         _chart.UpdateCurveColorAndStyle(curve, dataColumn, DataColumns);

         if (defaultCurveOptions != null)
            curve.CurveOptions.UpdateFrom(defaultCurveOptions);

         _chart.AddCurve(curve);
         return curve;
      }

      public IEnumerable<AxisTypes> GetAxisTypes()
      {
         var availableTypes = _chart.Axes.Keys.ToList();
         // show only yAxisTypes
         availableTypes.Remove(AxisTypes.X);
         return availableTypes;
      }

      public void SetCurveXData(ICurve curve, string columnId)
      {
         _chart.SetxData(curve, DataColumns[columnId], _dimensionFactory);
      }

      public void SetCurveYData(ICurve curve, string columnId)
      {
         _chart.SetyData(curve, DataColumns[columnId], _dimensionFactory);
      }

      private void syncUnitsAndCheckDimension(ICurve curve, IAxis axis)
      {
         string errorInfo = string.Empty;
         if (axis.AxisType == AxisTypes.X)
         {
            if (!curve.XDimension.HasSharedUnitNamesWith(axis.Dimension))
               errorInfo = Error.DifferentXAxisDimension;

            curve.ErrorInfos[c => c.xData] = errorInfo;
         }
         else if (axis.AxisType == curve.yAxisType)
         {
            if (!curve.YDimension.HasSharedUnitNamesWith(axis.Dimension))
               errorInfo = Error.DifferentYAxisDimension;

            else if (!curve.YDimension.CanConvertToUnit(axis.UnitName))
               errorInfo = Error.CannotConvertYAxisUnits;

            curve.ErrorInfos[c => c.yData] = errorInfo;
            curve.ErrorInfos[c => c.yAxisType] = errorInfo;
         }

         updateDisplayUnitInCurveData(curve, axis);
      }

      private void updateDisplayUnitInCurveData(ICurve curve, IAxis axis)
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