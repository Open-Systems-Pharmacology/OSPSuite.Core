using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class AbstractParameterIdentificationTimeProfileConfidenceIntervalChartPresenter<TChart> : ParameterIdentificationSingleRunAnalysisPresenter<TChart> where TChart : ParameterIdentificationAnalysisChartWithLocalRepositories, ISimulationAnalysis
   {
      private readonly ITimeProfileConfidenceIntervalCalculator _timeProfileConfidenceIntervalCalculator;
      private readonly Func<ITimeProfileConfidenceIntervalCalculator, Func<ParameterIdentification, ParameterIdentificationRunResult, IReadOnlyList<DataRepository>>> _confidenceIntervalFunc;

      public AbstractParameterIdentificationTimeProfileConfidenceIntervalChartPresenter(IParameterIdentificationSingleRunAnalysisView view,
         ChartPresenterContext chartPresenterContext,
         ITimeProfileConfidenceIntervalCalculator timeProfileConfidenceIntervalCalculator, ApplicationIcon icon, string presentationKey, Func<ITimeProfileConfidenceIntervalCalculator, Func<ParameterIdentification, ParameterIdentificationRunResult, IReadOnlyList<DataRepository>>> confidenceIntervalFunc) :
            base(view, chartPresenterContext, icon, presentationKey)
      {
         _timeProfileConfidenceIntervalCalculator = timeProfileConfidenceIntervalCalculator;
         _confidenceIntervalFunc = confidenceIntervalFunc;
      }

      protected override void AddRunResultToChart(ParameterIdentificationRunResult selectedRunResults)
      {
         if (ChartIsBeingCreated || ChartIsBeingUpdated)
         {
            var confidenceIntervalRepositories = _confidenceIntervalFunc(_timeProfileConfidenceIntervalCalculator)(_parameterIdentification, selectedRunResults);
            Chart.ClearDataRepositories();
            if (confidenceIntervalRepositories.Any())
            {
               AddUsedObservedDataToChart();
               Chart.AddRepositories(confidenceIntervalRepositories);
                  resetChartTitleIfRequired();
            }
            else
               Chart.Title = Captions.ParameterIdentification.ConfidenceIntervalNotAvailable;

            if (ChartIsBeingCreated)
               addDefaultCurves();
         }

         AddResultRepositoriesToEditor(Chart.DataRepositories);
         UpdateChartFromTemplate();
      }

      private void resetChartTitleIfRequired()
      {
         if (string.Equals(Chart.Title, Captions.ParameterIdentification.ConfidenceIntervalNotAvailable))
            Chart.Title = string.Empty;
      }

      private void addDefaultCurves()
      {
         Chart.DataRepositories.Each(x => AddCurvesFor(x.AllButBaseGrid().Where(col => !col.IsInternal), (col, curve) =>
         {
            SelectColorForCalculationColumn(col);
            UpdateColorForCalculationColumn(curve, col);
         }));
      }
   }
}