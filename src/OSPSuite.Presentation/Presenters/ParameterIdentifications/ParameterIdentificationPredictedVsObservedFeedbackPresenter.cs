using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationPredictedVsObservedFeedbackPresenter : IParameterIdentificationChartFeedbackPresenter
   {
   }

   public class ParameterIdentificationPredictedVsObservedFeedbackPresenter : ParameterIdentificationChartFeedbackPresenter<ParameterIdentificationPredictedVsObservedChart>, IParameterIdentificationPredictedVsObservedFeedbackPresenter
   {
      private readonly IPredictedVsObservedChartService _predictedVsObservedChartService;

      public ParameterIdentificationPredictedVsObservedFeedbackPresenter(IParameterIdentificationChartFeedbackView view, IChartDisplayPresenter chartDisplayPresenter, IDimensionFactory dimensionFactory,
         IDisplayUnitRetriever displayUnitRetriever, IPredictedVsObservedChartService predictedVsObservedChartService) :
         base(view, chartDisplayPresenter, dimensionFactory, displayUnitRetriever, new ParameterIdentificationPredictedVsObservedChart {Title = Captions.ParameterIdentification.PredictedVsObservedAnalysis})
      {
         _predictedVsObservedChartService = predictedVsObservedChartService;
      }

      public override void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         base.EditParameterIdentification(parameterIdentification);

         addIdentityCurve();

         _view.BindToSelecteOutput();
      }

      protected override void AddCurvesFor(DataRepository repository, Action<DataColumn, Curve> action)
      {
         plotAllCalculations(repository, (column, curve) =>
         {
            action(column, curve);
            curve.VisibleInLegend = _chart.Curves.Count(x => Equals(x.Name, repository.Name)) == 1;
         });
      }

      private void plotAllCalculations(DataRepository dataRepository, Action<DataColumn, Curve> action)
      {
         var calculationColumns = dataRepository.AllButBaseGrid();
         var allObservationsForOutput = _parameterIdentification.AllObservationColumnsFor(SelectedOutput.FullOutputPath);
         calculationColumns.Each(calculationColumn =>
         {
            _predictedVsObservedChartService.AddCurvesFor(allObservationsForOutput, calculationColumn, _chart, action);
         });
      }

      protected override void UpdateChartForSelectedOutput()
      {
         _chart.Clear();
         ConfigureColumnDimension(_bestColumn);
         ConfigureColumnDimension(_currentColumn);
         AddBestAndCurrent();
         addIdentityCurve();
         UpdateChartAxesScalings();
      }

      protected override void UpdateChartAxesScalings()
      {
         _chart.Axes.Each(axis => axis.Scaling = SelectedOutput.Scaling);
      }

      private void addIdentityCurve()
      {
         var allObservationColumnsFor = _parameterIdentification.AllObservationColumnsFor(SelectedOutput.FullOutputPath).ToList();
         _predictedVsObservedChartService.AddIdentityCurves(allObservationColumnsFor, _chart);
         _predictedVsObservedChartService.SetXAxisDimension(allObservationColumnsFor, _chart);
      }
   }
}