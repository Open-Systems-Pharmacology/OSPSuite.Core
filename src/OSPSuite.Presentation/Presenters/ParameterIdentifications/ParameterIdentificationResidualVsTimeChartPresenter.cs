using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
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

      public ParameterIdentificationResidualVsTimeChartPresenter(
         IParameterIdentificationSingleRunAnalysisView view,
         ChartPresenterContext chartPresenterContext,
         IResidualsVsTimeChartService residualsVsTimeChartService) :
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
         AddDataRepositoriesToEditor(new[] {_zeroRepository});

         if (ChartIsBeingCreated)
            _residualsVsTimeChartService.ConfigureChartAxis(Chart);

         UpdateChartFromTemplate();
      }

      protected override void AddRunResultToChart(ParameterIdentificationRunResult runResult)
      {
         runResult.BestResult.AllOutputResiduals.GroupBy(x => x.FullOutputPath).Each(x => addOutputToScatter(x, runResult));
      }

      private void addOutputToScatter(IGrouping<string, OutputResiduals> outputMappingsByOutput, ParameterIdentificationRunResult runResult)
      {
         var fullOutputPath = outputMappingsByOutput.Key;
         var shouldShowInLegend = true;
         foreach (var outputMapping in outputMappingsByOutput)
         {
            var dataRepository = getOrCreateScatterDataRepositoryFor(runResult.Index, outputMapping);
            AddResultRepositoryToEditor(dataRepository);
            var visibleInLegend = shouldShowInLegend;

            AddCurvesFor(dataRepository, (column, curve) =>
            {
               SelectColorForPath(fullOutputPath);
               UpdateColorForPath(curve, fullOutputPath);
               curve.Name = NameForColumn(column);
               curve.Description = CurveDescription(outputMapping.ObservedDataName, runResult);
               curve.Symbol = Symbols.Circle;
               curve.LineStyle = LineStyles.None;
               curve.VisibleInLegend = visibleInLegend;
            });

            shouldShowInLegend = false;
         }
      }

      private DataRepository getOrCreateScatterDataRepositoryFor(int runIndex, OutputResiduals outputResidual) =>
         _residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(Chart, outputResidual, runIndex);

      public override void Clear()
      {
         base.Clear();
         Chart.RemoveCurvesForDataRepository(_zeroRepository);
      }
   }
}