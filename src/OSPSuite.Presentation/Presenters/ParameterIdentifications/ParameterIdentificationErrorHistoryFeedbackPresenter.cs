using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationErrorHistoryFeedbackPresenter : IParameterIdentificationRunFeedbackPresenter
   {
      CurveChart Chart { get; }
   }

   public class ParameterIdentificationErrorHistoryFeedbackPresenter : AbstractPresenter<IParameterIdentificationErrorHistoryFeedbackView, IParameterIdentificationErrorHistoryFeedbackPresenter>, IParameterIdentificationErrorHistoryFeedbackPresenter
   {
      private readonly IChartDisplayPresenter _chartDisplayPresenter;
      private readonly IDimensionFactory _dimensionFactory;
      private DataRepository _errorRepository;
      private DataColumn _errorColumn;
      public CurveChart Chart { get; }

      public ParameterIdentificationErrorHistoryFeedbackPresenter(IParameterIdentificationErrorHistoryFeedbackView view, IChartDisplayPresenter chartDisplayPresenter, IDimensionFactory dimensionFactory) : base(view)
      {
         _chartDisplayPresenter = chartDisplayPresenter;
         _dimensionFactory = dimensionFactory;
         _view.AddChartView(_chartDisplayPresenter.View);
         AddSubPresenters(_chartDisplayPresenter);
         Chart = new CurveChart().WithAxes();
         Chart.Title = Captions.ParameterIdentification.ErrorHistory;
         _chartDisplayPresenter.Edit(Chart, new ChartFontAndSizeSettings().ForParameterIdentificationFeedback());
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _errorRepository = createErrorRepository();
         initializeChart();
      }

      private void initializeChart()
      {
         AddCurvesFor(_errorRepository, (col, curve) =>
         {
            _errorColumn = col;
            curve.Color = Colors.ErrorColor;
            curve.LineThickness = 2;
            curve.VisibleInLegend = false;
         });

         Chart.AxisBy(AxisTypes.X).Caption = Captions.ParameterIdentification.NumberOfEvaluations;
         Chart.AxisBy(AxisTypes.Y).Caption = Captions.ParameterIdentification.TotalError;
         Chart.AxisBy(AxisTypes.Y).Scaling = Scalings.Linear;
      }

      protected void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, Curve> action = null)
      {
         Chart.AddCurvesFor(dataRepository, x => x.Name, _dimensionFactory, action);
      }

      public void ClearReferences()
      {
         //nothing to do here. No reference to actual PI is saved
      }

      public void ResetFeedback()
      {
         Chart.Clear();
         _chartDisplayPresenter.Refresh();
      }

      public void UpdateFeedback(ParameterIdentificationRunState runState)
      {
         updateRepository(runState.ErrorHistory);
         _chartDisplayPresenter.Refresh();
      }

      private void updateRepository(IReadOnlyList<float> errorHistory)
      {
         _errorColumn.BaseGrid.Values = Enumerable.Range(1, errorHistory.Count).Select(Convert.ToSingle).ToList();
         _errorColumn.Values = errorHistory;
      }

      private DataRepository createErrorRepository()
      {
         var dataRepository = new DataRepository {Name = Captions.ParameterIdentification.TotalError};
         var baseGrid = new BaseGrid(Captions.ParameterIdentification.NumberOfEvaluations, Constants.Dimension.NO_DIMENSION)
         {
            Values = new[] {0f}
         };

         var value = new DataColumn(Captions.ParameterIdentification.TotalError, Constants.Dimension.NO_DIMENSION, baseGrid)
         {
            Values = new[] {0f}
         };

         dataRepository.Add(value);
         return dataRepository;
      }
   }
}