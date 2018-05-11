using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility;
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

   public class CurveEventArgs : EventArgs
   {
      public Curve Curve { get; }
      public CurveEventArgs(Curve curve) => Curve = curve;
   }

   public interface ICurveSettingsPresenter : IPresenter<ICurveSettingsView>, IPresenterWithColumnSettings
   {
      void Edit(CurveChart chart);
      void Clear();
      Func<DataColumn, string> CurveNameDefinition { set; get; }

      void Remove(CurveDTO curve);
      event EventHandler<CurveEventArgs> RemoveCurve;

      void AddCurvesForColumns(IReadOnlyList<DataColumn> dataColumns);
      event EventHandler<ColumnsEventArgs> AddCurves;

      void SetCurveXData(CurveDTO curveDTO, DataColumn dataColumn);
      void SetCurveYData(CurveDTO curveDTO, DataColumn dataColumn);

      IEnumerable<AxisTypes> AllYAxisTypes { get; }
      string ToolTipFor(CurveDTO curveDTO);
      void MoveCurvesInLegend(CurveDTO curveBeingMoved, CurveDTO targetCurve);
      void Refresh();

      void NotifyCurvePropertyChange(CurveDTO curveDTO);
      event EventHandler<CurveEventArgs> CurvePropertyChanged;
      void UpdateCurveColor(CurveDTO curveDTO, Color color);
   }

   public class CurveSettingsPresenter : PresenterWithColumnSettings<ICurveSettingsView, ICurveSettingsPresenter>, ICurveSettingsPresenter, ILatchable
   {
      public event EventHandler<CurveEventArgs> RemoveCurve = delegate { };
      public event EventHandler<ColumnsEventArgs> AddCurves = delegate { };
      public event EventHandler<CurveEventArgs> CurvePropertyChanged = delegate { };

      private readonly IDimensionFactory _dimensionFactory;

      public Func<DataColumn, string> CurveNameDefinition { get; set; }
      private CurveChart _chart;
      private readonly List<CurveDTO> _allCurvesDTOs = new List<CurveDTO>();
      public bool IsLatched { get; set; }

      public IEnumerable<AxisTypes> AllYAxisTypes => _chart.AllUsedYAxisTypes;

      public CurveSettingsPresenter(ICurveSettingsView view, IDimensionFactory dimensionFactory) : base(view)
      {
         _dimensionFactory = dimensionFactory;
         CurveNameDefinition = column => column.Name;
      }

      public void MoveCurvesInLegend(CurveDTO curveBeingMoved, CurveDTO targetCurve)
      {
         _chart.MoveCurvesInLegend(curveBeingMoved.Curve, targetCurve.Curve);
         NotifyCurvePropertyChange(targetCurve);
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

      public void Remove(CurveDTO curveDTO) => RemoveCurve(this, new CurveEventArgs(curveDTO.Curve));

      public void AddCurvesForColumns(IReadOnlyList<DataColumn> dataColumns) => AddCurves(this, new ColumnsEventArgs(dataColumns));

      private CurveDTO mapFrom(Curve curve) => new CurveDTO(curve, _chart);

      public void Refresh()
      {
         //Refresh initiated from UI action
         if(IsLatched)
            return;

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
         this.DoWithinLatch(() =>
            CurvePropertyChanged(this, new CurveEventArgs(curveDTO.Curve))
         );
      }

      private void rebind()
      {
         View.BindTo(_allCurvesDTOs);
      }

      private void addCurve(Curve curve) => _allCurvesDTOs.Add(mapFrom(curve));

      private bool hasCurveDTOFor(Curve curve)
      {
         return _allCurvesDTOs.Any(x => Equals(x.Curve, curve));
      }

      public void SetCurveXData(CurveDTO curveDTO, DataColumn dataColumn)
      {
         _chart.SetxData(curveDTO.Curve, dataColumn, _dimensionFactory);
         NotifyCurvePropertyChange(curveDTO);
      }

      public void SetCurveYData(CurveDTO curveDTO, DataColumn dataColumn)
      {
         _chart.SetyData(curveDTO.Curve, dataColumn, _dimensionFactory);
         NotifyCurvePropertyChange(curveDTO);
      }

      public void UpdateCurveColor(CurveDTO curveDTO, Color color)
      {
         curveDTO.Color = color;
         NotifyCurvePropertyChange(curveDTO);
      }
   }
}