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
   }

   public class ParameterIdentificationErrorHistoryFeedbackPresenter : AbstractPresenter<IParameterIdentificationErrorHistoryFeedbackView, IParameterIdentificationErrorHistoryFeedbackPresenter>, IParameterIdentificationErrorHistoryFeedbackPresenter
   {
      private readonly IChartDisplayPresenter _chartDisplayPresenter;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly CurveChart _chart;
      private DataRepository _errorRepository;
      private DataColumn _errorColumn;

      public ParameterIdentificationErrorHistoryFeedbackPresenter(IParameterIdentificationErrorHistoryFeedbackView view, IChartDisplayPresenter chartDisplayPresenter, IDimensionFactory dimensionFactory) : base(view)
      {
         _chartDisplayPresenter = chartDisplayPresenter;
         _dimensionFactory = dimensionFactory;
         _view.AddChartView(_chartDisplayPresenter.View);
         AddSubPresenters(_chartDisplayPresenter);
         _chart = new CurveChart().WithAxes();
         _chart.Title = Captions.ParameterIdentification.ErrorHistory;
         _chart.FontAndSize.Fonts.TitleSize = Constants.ChartFontOptions.DefaultFontSizeTitleForParameterIdentificationFeedback;
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _errorRepository = createErrorRepository();
         initializeChart();
      }

      private void initializeChart()
      {
         AddCurvesFor(_errorRepository,  (col, curve) =>
         {
            _errorColumn = col;
            curve.Color = Colors.ErrorColor;
            curve.LineThickness = 2;
            curve.VisibleInLegend = false;
         });

         _chart.Axes[AxisTypes.X].Caption = Captions.ParameterIdentification.NumberOfEvaluations;
         _chart.Axes[AxisTypes.Y].Caption = Captions.ParameterIdentification.TotalError;
         _chart.Axes[AxisTypes.Y].Scaling = Scalings.Linear;

         _chartDisplayPresenter.DataSource = _chart;
         _chartDisplayPresenter.View.SetFontAndSizeSettings(_chart.FontAndSize);
      }

      protected void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, ICurve> action=null)
      {
         _chart.AddCurvesFor(dataRepository, x => x.Name, _dimensionFactory,  action);
      }

      public void ClearReferences()
      {
      }

      public void ResetFeedback()
      {
         _chart.Clear();
         _chartDisplayPresenter.DataSource = null;
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