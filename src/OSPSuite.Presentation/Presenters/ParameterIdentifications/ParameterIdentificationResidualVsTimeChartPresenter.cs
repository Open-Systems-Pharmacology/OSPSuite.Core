using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationResidualVsTimeChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationResidualVsTimeChartPresenter : ParameterIdentificationSingleRunAnalysisPresenter<ParameterIdentificationResidualVsTimeChart>, IParameterIdentificationResidualVsTimeChartPresenter
   {
      private readonly IResidualsVsTimeChartService _residualsVsTimeChartService;
      private DataRepository _zeroRepository;

      public ParameterIdentificationResidualVsTimeChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext, IResidualsVsTimeChartService residualsVsTimeChartService) :
         base(view, chartPresenterContext, ApplicationIcons.ResidualVsTimeAnalysis, PresenterConstants.PresenterKeys.ParameterIdentificationResidualVsTimeChartPresenter)
      {
         _residualsVsTimeChartService = residualsVsTimeChartService;
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         base.UpdateAnalysisBasedOn(parameterIdentificationResults);
         if (!_parameterIdentification.AllObservedData.Any())
            return;

         _zeroRepository = _residualsVsTimeChartService.AddZeroMarkerCurveToChart(Chart, _parameterIdentification.MinObservedDataTime, _parameterIdentification.MaxObservedDataTime);
         AddDataRepositoriesToEditor(new[] { _zeroRepository });

         if (ChartIsBeingCreated)
         {
            Chart.AxisBy(AxisTypes.Y).Caption = Captions.ParameterIdentification.Residuals;
            Chart.AxisBy(AxisTypes.Y).Scaling = Scalings.Linear;
         }

         UpdateChartFromTemplate();
      }

      protected override void AddRunResultToChart(ParameterIdentificationRunResult runResult)
      {
         runResult.BestResult.AllOutputResiduals.GroupBy(x => x.FullOutputPath).Each(x => addOutputToScatter(x, runResult));
      }

      private void addOutputToScatter(IGrouping<string, OutputResiduals> outputMappingsByOutput, ParameterIdentificationRunResult runResult)
      {
         var fullOutputPath = outputMappingsByOutput.Key;
         bool shouldShowInLegend = true;
         foreach (var outputMapping in outputMappingsByOutput)
         {
            var dataRepository = getOrCreateScatterDataRepositoryFor(runResult.Index, outputMapping);
            AddResultRepositoryToEditor(dataRepository);
            var visibleInLegend = shouldShowInLegend;

            AddCurvesFor(dataRepository, (column, curve) =>
            {
               SelectColorForPath(fullOutputPath);
               UpdateColorForPath(curve, fullOutputPath);
               curve.Name = fullOutputPath;
               curve.Description = CurveDescription(outputMapping.ObservedDataName, runResult);
               curve.Symbol = Symbols.Circle;
               curve.LineStyle = LineStyles.None;
               curve.VisibleInLegend = visibleInLegend;
            });

            shouldShowInLegend = false;
         }
      }


      private DataRepository getOrCreateScatterDataRepositoryFor(int runIndex, OutputResiduals outputResidual)
      {
         var repositoryName = Captions.ParameterIdentification.SimulationResultsForRun(runIndex);
         var id = $"{Chart.Id}-{outputResidual.FullOutputPath}-{outputResidual.ObservedData.Id}-{runIndex}";

         var timeValues = outputResidual.Residuals.Select(x => x.Time).ToList();
         var outputValues = outputResidual.Residuals.Select(x => x.Value).ToList();

         var dataRepository = Chart.DataRepositories.FindById(id);
         if (dataRepository == null)
         {
            dataRepository = _residualsVsTimeChartService.CreateScatterDataRepository(id, repositoryName, outputResidual);
            Chart.AddRepository(dataRepository);
         }

         dataRepository.BaseGrid.Values = timeValues.ToFloatArray();
         dataRepository.FirstDataColumn().Values = outputValues.ToFloatArray();

         return dataRepository;
      }

      public override void Clear()
      {
         base.Clear();
         Chart.RemoveCurvesForDataRepository(_zeroRepository);
      }
   }
}