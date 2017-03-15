using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Chart
{
   public interface ICurveChart : INotifier, IWithId, IChart, IVisitable<IVisitor>
   {
      IItemNotifyCache<AxisTypes, IAxis> Axes { get; }
      IItemNotifyCache<string, ICurve> Curves { get; }

      /// <summary>
      ///    Checks for data of curve and adds to curve collection.
      /// </summary>
      /// <param name="curve"> Curve to be added </param>
      /// <param name="useAxisDefault"> (default) true, if AxisDefaults should be applied </param>
      void AddCurve(ICurve curve, bool useAxisDefault = true);

      /// <summary>
      ///    Removes curve and removes dimension.
      /// </summary>
      /// <param name="curveId"> Curve Id to be removed. </param>
      void RemoveCurve(string curveId);

      void Clear();

      void CopyChartSettingsFrom(IChartManagement chart);

      /// <summary>
      ///    Removes all Curves with x-/y-Data empty or outside a DataRepository.
      /// </summary>
      void RemoveDatalessCurves();

      /// <summary>
      ///    Removes all Curves with x-/y-Data in passed DataRepository (or empty).
      /// </summary>
      /// <param name="dataRepository"></param>
      void RemoveCurvesForDataRepository(DataRepository dataRepository);

      /// <summary>
      ///    Removes all Curves with x-/y-Data with passed ColumnId.
      /// </summary>
      /// <param name="columnId"></param>
      void RemoveCurvesForColumn(string columnId);

      IReadOnlyCollection<string> UsedColumnIds { get; }

      // the following methods update the axes dimension and unitName, if possible
      ICurve CreateCurve(DataColumn columnX, DataColumn columnY, string curveName, IDimensionFactory dimensionFactory);

      void SetxData(ICurve curve, DataColumn column, IDimensionFactory dimensionFactory);
      void SetyData(ICurve curve, DataColumn column, IDimensionFactory dimensionFactory);

      /// <summary>
      ///    Gives the default scaling for newly create Y axes
      /// </summary>
      Scalings DefaultYAxisScaling { set; get; }

      event EventHandler StartUpdateEvent;
      event EventHandler EndUpdateEvent;

      void MoveSeriesInLegend(ICurve movingCurve, ICurve targetCurve);
      ICurve FindCurveWithSameData(DataColumn xData, DataColumn yData);
   }

   public class CurveChart : Notifier, ICurveChart
   {
      public event EventHandler StartUpdateEvent = delegate { };
      public event EventHandler EndUpdateEvent = delegate { };
      private readonly IItemNotifyCache<string, ICurve> _curves;
      private readonly IItemNotifyCache<AxisTypes, IAxis> _axes;
      public string Id { get; set; }
      private bool _previewSettings;
      private bool _includeOriginData;
      private string _description;
      private string _name;
      private string _title;
      private ChartFontAndSizeSettings _fontAndSize;
      private ChartSettings _chartSettings;
      private string _originText;
      public Scalings DefaultYAxisScaling { get; set; }

      public CurveChart()
      {
         Id = string.Empty;
         _name = string.Empty;
         _description = string.Empty;
         _title = string.Empty;

         _axes = new ItemNotifyCache<AxisTypes, IAxis>(axis => axis.AxisType);
         _axes.CollectionChanged += (o, args) => OnPropertyChanged(() => Axes);
         _axes.ItemChanged += (o, args) => OnPropertyChanged(() => Axes);

         _curves = new ItemNotifyCache<string, ICurve>(curve => curve.Id);
         _curves.CollectionChanged += (o, args) => OnPropertyChanged(() => Curves);
         _curves.ItemChanged += (o, args) => OnPropertyChanged(() => Curves);

         FontAndSize = new ChartFontAndSizeSettings();
         ChartSettings = new ChartSettings();
         DefaultYAxisScaling = Scalings.Log;
      }

      public ChartFontAndSizeSettings FontAndSize
      {
         get { return _fontAndSize; }
         private set
         {
            _fontAndSize = value;
            _fontAndSize.PropertyChanged += (o, e) => OnPropertyChanged(() => FontAndSize);
         }
      }

      public ChartSettings ChartSettings
      {
         get { return _chartSettings; }
         private set
         {
            _chartSettings = value;
            _chartSettings.PropertyChanged += (o, e) => OnPropertyChanged(() => ChartSettings);
         }
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

      public string OriginText
      {
         get { return _originText; }
         set
         {
            _originText = value;
            OnPropertyChanged(() => OriginText);
         }
      }

      public bool IncludeOriginData
      {
         get { return _includeOriginData; }
         set
         {
            _includeOriginData = value;
            OnPropertyChanged(() => IncludeOriginData);
         }
      }

      public bool PreviewSettings
      {
         get { return _previewSettings; }
         set
         {
            _previewSettings = value;
            OnPropertyChanged(() => PreviewSettings);
         }
      }

      public void MoveSeriesInLegend(ICurve movingCurve, ICurve targetCurve)
      {
         var index = targetCurve.LegendIndex;
         try
         {
            StartUpdateEvent(this, EventArgs.Empty);
            Curves.Where(curve => curve.LegendIndex >= index).Each(curve => curve.LegendIndex++);
         }
         finally
         {
            EndUpdateEvent(this, new EventArgs());
         }
         movingCurve.LegendIndex = index;
      }

      public IItemNotifyCache<AxisTypes, IAxis> Axes => _axes;

      public IItemNotifyCache<string, ICurve> Curves => _curves;

      public ICurve CreateCurve(DataColumn columnX, DataColumn columnY, string curveName, IDimensionFactory dimensionFactory)
      {
         var curve = FindCurveWithSameData(columnX, columnY);
         if (curve != null)
            return curve;

         curve = new Curve {Name = curveName};
         curve.SetxData(columnX, dimensionFactory);
         curve.SetyData(columnY, dimensionFactory);
         return curve;
      }

      public virtual string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged(() => Name);
         }
      }

      public virtual string Description
      {
         get { return _description; }
         set
         {
            _description = value;
            OnPropertyChanged(() => Description);
         }
      }

      public string Title
      {
         get { return _title; }
         set
         {
            _title = value;
            OnPropertyChanged(() => Title);
         }
      }

      public void SetxData(ICurve curve, DataColumn column, IDimensionFactory dimensionFactory)
      {
         curve.SetxData(column, dimensionFactory);
         updateAxesForAddedCurve(curve);
      }

      public void SetyData(ICurve curve, DataColumn column, IDimensionFactory dimensionFactory)
      {
         curve.SetyData(column, dimensionFactory);
         updateAxesForAddedCurve(curve);
      }

      public ICurve FindCurveWithSameData(DataColumn xData, DataColumn yData)
      {
         return Curves.FirstOrDefault(curve => xData.Id == curve.xData.Id && yData.Id == curve.yData.Id);
      }

      public void AddCurve(ICurve curve, bool useAxisDefault = true)
      {
         if (curve == null) return;
         if (Curves.Contains(curve.Id)) return;
         if (curve.xData == null) throw new MissingDataException("x", curve.Name);
         if (curve.yData == null) throw new MissingDataException("y", curve.Name);
         updateAxesForAddedCurve(curve);

         if (useAxisDefault)
            copyLineStyleAndColorFromYAxisDefault(curve);

         Curves.Add(curve);

         if (!curve.LegendIndex.HasValue)
            curve.LegendIndex = Curves.Count;
      }

      private void copyLineStyleAndColorFromYAxisDefault(ICurve curve)
      {
         var axis = Axes[curve.CurveOptions.yAxisType];
         if (axis.DefaultLineStyle != LineStyles.None && curve.Symbol == Symbols.None)
            curve.LineStyle = axis.DefaultLineStyle;

         if (!isWhite(axis.DefaultColor))
            curve.Color = axis.DefaultColor;
      }

      private bool isWhite(Color color)
      {
         return color.R == 255 && color.G == 255 && color.B == 255;
      }

      public void AddCurveIfConsistent(ICurve curve)
      {
         if (curve?.xData == null || curve.yData == null) return;
         AddCurve(curve, useAxisDefault: false);
      }

      private void updateAxesForAddedCurve(ICurve curve)
      {
         updateAxis(AxisTypes.X, curve.XDimension, curve.xData.DataInfo.DisplayUnitName);

         var axisTypeY = curve.CurveOptions.yAxisType;
         var axisTypeYother = axisTypeY == AxisTypes.Y ? AxisTypes.Y2 : AxisTypes.Y;

         var yAxis = updateAxis(axisTypeY, curve.YDimension, curve.yData.DataInfo.DisplayUnitName);

         if (curve.YDimension.HasSharedUnitNamesWith(yAxis.Dimension))
            return;

         var y2Axis = updateAxis(axisTypeYother, curve.YDimension, curve.yData.DataInfo.DisplayUnitName);

         if (curve.YDimension.HasSharedUnitNamesWith(y2Axis.Dimension))
            curve.yAxisType = axisTypeYother;
      }

      private IAxis updateAxis(AxisTypes axisType, IDimension dimension, string displayUnitName)
      {
         if (!Axes.Contains(axisType))
         {
            var newAxis = new Axis(axisType);
            Axes.Add(newAxis);

            if (newAxis.IsYAxis())
               newAxis.Scaling = DefaultYAxisScaling;
         }

         var axis = Axes[axisType];
         if (axis.Dimension == null)
         {
            axis.Dimension = dimension;
            setAxisUnitName(axis, displayUnitName);
         }

         return axis;
      }

      private void setAxisUnitName(IAxis axis, string dataUnitName)
      {
         axis.UnitName = axis.Dimension.UnitOrDefault(dataUnitName).Name;
      }

      public void RemoveCurve(string curveId)
      {
         if (!Curves.Contains(curveId)) return;
         var yAxisType = Curves[curveId].yAxisType;
         Curves.Remove(curveId);
         updateAxesForRemovedCurve(yAxisType);
      }

      private void updateAxesForRemovedCurve(AxisTypes yAxisType)
      {
         if (!Curves.Any())
         {
            Axes[AxisTypes.X].Dimension = null;
         }
         if (Curves.All(c => c.yAxisType != yAxisType))
         {
            Axes[yAxisType].Dimension = null;
         }
      }

      public void RemoveDatalessCurves()
      {
         removeCurves(curve => !curve.xData.IsInRepository() || !curve.yData.IsInRepository());
      }

      public void RemoveCurvesForColumn(string columnId)
      {
         removeCurves(curve => curve.xData.Id == columnId || curve.yData.Id == columnId);
      }

      public void RemoveCurvesForDataRepository(DataRepository dataRepository)
      {
         removeCurves(curve => dataRepository.Contains(curve.xData.Id) || dataRepository.Contains(curve.yData.Id));
      }

      private void removeCurves(Func<ICurve, bool> shouldRemoveCurveFunc)
      {
         var curvesToRemove = Curves.Where(shouldRemoveCurveFunc).ToList();
         curvesToRemove.Each(x => RemoveCurve(x.Id));
      }

      public IReadOnlyCollection<string> UsedColumnIds
      {
         get
         {
            var usedColumnIds = new HashSet<string>();
            foreach (var curve in _curves)
            {
               if (curve.xData != null)
                  usedColumnIds.Add(curve.xData.Id);

               if (curve.yData != null)
                  usedColumnIds.Add(curve.yData.Id);
            }
            return usedColumnIds.ToList();
         }
      }

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}