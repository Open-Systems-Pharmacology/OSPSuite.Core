using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

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
         base(view, chartDisplayPresenter, dimensionFactory, displayUnitRetriever, new ParameterIdentificationPredictedVsObservedChart { Title = Captions.ParameterIdentification.PredictedVsObservedAnalysis })
      {
         _predictedVsObservedChartService = predictedVsObservedChartService;
      }

      public override void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         base.EditParameterIdentification(parameterIdentification);

         addIdentityCurve();

         _view.BindToSelecteOutput();

         _chartDisplayPresenter.Edit(_chart);
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
         calculationColumns.Each(calculationColumn =>
         {
            _predictedVsObservedChartService.AddCurvesFor(_parameterIdentification.AllObservationColumnsFor(SelectedOutput.FullOutputPath),
               calculationColumn, _chart, action);
         });
      }

      protected override void SelectedOutputChanged()
      {
         _chart.Clear();
         ConfigureColumnDimension(_bestColumn);
         ConfigureColumnDimension(_currentColumn);
         AddBestAndCurrent();
         addIdentityCurve();

         base.SelectedOutputChanged();
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