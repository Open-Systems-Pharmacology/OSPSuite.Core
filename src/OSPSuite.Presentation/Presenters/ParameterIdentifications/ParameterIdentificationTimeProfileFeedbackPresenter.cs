using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationTimeProfileFeedbackPresenter : IParameterIdentificationChartFeedbackPresenter
   {
   }

   public class ParameterIdentificationTimeProfileFeedbackPresenter : ParameterIdentificationChartFeedbackPresenter<CurveChart>, IParameterIdentificationTimeProfileFeedbackPresenter
   {
      private readonly List<Curve> _allObservedDataCurves;

      public ParameterIdentificationTimeProfileFeedbackPresenter(IParameterIdentificationChartFeedbackView view, IChartDisplayPresenter chartDisplayPresenter, IDimensionFactory dimensionFactory, IDisplayUnitRetriever displayUnitRetriever) :
         base(view, chartDisplayPresenter, dimensionFactory, displayUnitRetriever, new CurveChart {Title = Captions.ParameterIdentification.TimeProfileAnalysis })
      {
         _allObservedDataCurves = new List<Curve>();
      }

      public override void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         base.EditParameterIdentification(parameterIdentification);

         addObservedDataForSelectedOutput();

         _view.BindToSelecteOutput();
         _chartDisplayPresenter.Edit(_chart);
      }

      private void addObservedDataForSelectedOutput()
      {
         _allObservedDataCurves.Each(x => _chart.RemoveCurve(x.Id));
         _allObservedDataCurves.Clear();
         AddCurvesForCalculationColumns(_parameterIdentification.AllObservationColumnsFor(SelectedOutput.FullOutputPath), (col, curve) =>
         {
            _allObservedDataCurves.Add(curve);
            curve.VisibleInLegend = false;
            curve.Color = Color.LightGray;
         });
      }

      public override void ResetFeedback()
      {
         base.ResetFeedback();
         _allObservedDataCurves.Clear();
      }

      protected override void UpdateChartAxesScalings()
      {
         _chart.Axes.Where(axis => axis.AxisType != AxisTypes.X).Each(axis => axis.Scaling = SelectedOutput.Scaling);
      }

      protected override void AddCurvesFor(DataRepository repository, Action<DataColumn, Curve> action)
      {
         _chart.AddCurvesFor(repository, x => x.Name, _dimensionFactory, action);
      }

      protected override void SelectedOutputChanged()
      {
         base.SelectedOutputChanged();
         //TODO
//         using (new BatchUpdate(_chartDisplayPresenter))
//         {
            ConfigureColumnDimension(_currentColumn);
            ConfigureColumnDimension(_bestColumn);
            configureYAxisDimension();
//         }

         addObservedDataForSelectedOutput();
      }

      private void configureCurveForColumn(DataColumn column, IDimension dimension)
      {
         var curve = _chart.FindCurveWithSameData(column.BaseGrid, column);
         curve.YDimension = dimension;
      }

      private void configureYAxisDimension()
      {
         var yAxis = _chart.Axes.FirstOrDefault(x => x.AxisType == AxisTypes.Y);
         if (yAxis == null) return;
         yAxis.Dimension = _dimensionFactory.GetMergedDimensionFor(_bestColumn);
         yAxis.UnitName = _displayUnitRetriever.PreferredUnitFor(_bestColumn).Name;

         configureCurveForColumn(_bestColumn, yAxis.Dimension);
         configureCurveForColumn(_currentColumn, yAxis.Dimension);
      }

      protected void AddCurvesForCalculationColumns(IEnumerable<DataColumn> columns, Action<DataColumn, Curve> action)
      {
         _chart.AddCurvesFor(columns, x => x.Name, _dimensionFactory, action);
      }
   }
}