using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationResidualVsTimeChartPresenter : IParameterIdentificationSingleRunAnalysisPresenter
   {
   }

   public class ParameterIdentificationResidualVsTimeChartPresenter : ParameterIdentificationSingleRunAnalysisPresenter<ParameterIdentificationResidualVsTimeChart>, IParameterIdentificationResidualVsTimeChartPresenter
   {
      private string _markerCurveId = string.Empty;
      private const string ZERO = "Zero";

      public ParameterIdentificationResidualVsTimeChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext) :
            base(view, chartPresenterContext, ApplicationIcons.ResidualVsTimeAnalysis, PresenterConstants.PresenterKeys.ParameterIdentificationResidualVsTimeChartPresenter)
      {
         view.HelpId = HelpId.Tool_PI_Analysis_ResidualsVsTime;
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         base.UpdateAnalysisBasedOn(parameterIdentificationResults);
         if (!_parameterIdentification.AllObservedData.Any())
            return;

         addZeroMarkerCurveToChart();

         if (ChartIsBeingCreated)
         {
            Chart.Axes[AxisTypes.Y].Caption = Captions.ParameterIdentification.Residuals;
            Chart.Axes[AxisTypes.Y].Scaling = Scalings.Linear;
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
               SelectColorForCalculationColumn(column);
               UpdateColorForCalculationColumn(curve, column);
               curve.Name = fullOutputPath;
               curve.Description = CurveDescription(outputMapping.ObservedDataName, runResult);
               curve.Symbol = Symbols.Circle;
               curve.LineStyle = LineStyles.None;
               curve.VisibleInLegend = visibleInLegend;
            });

            shouldShowInLegend = false;
         }
      }

      private void addZeroMarkerCurveToChart()
      {
         var markerRepository = createMarkerRepository();
         AddDataRepositoryToEditor(markerRepository);
         AddCurvesFor(markerRepository, action: (column, curve) =>
         {
            curve.UpdateMarkerCurve(ZERO);
            _markerCurveId = curve.Id;
         });
      }

      private DataRepository createMarkerRepository()
      {
         var id = $"{Chart.Id}-{ZERO}";
         var dataRepository = createEmptyRepository(id, ZERO, ZERO);
         dataRepository.BaseGrid.Values = new[] { _parameterIdentification.MinObservedDataTime, _parameterIdentification.MaxObservedDataTime };
         dataRepository.FirstDataColumn().Values = new[] { 0f, 0f };
         return dataRepository;
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
            dataRepository = createScatterDataRepository(id, repositoryName, outputResidual);
            Chart.AddRepository(dataRepository);
         }

         dataRepository.BaseGrid.Values = timeValues.ToFloatArray();
         dataRepository.FirstDataColumn().Values = outputValues.ToFloatArray();

         return dataRepository;
      }

      private DataRepository createScatterDataRepository(string id, string repositoryName, OutputResiduals outputResidual)
      {
         var dataRepository = createEmptyRepository(id, repositoryName, "Values");
         var scatterColumn = dataRepository.FirstDataColumn();
         scatterColumn.QuantityInfo.Path = outputResidual.FullOutputPath.ToPathArray();
         return dataRepository;
      }

      private DataRepository createEmptyRepository(string id, string name, string valueName)
      {
         var dataRepository = new DataRepository(id) { Name = name };
         var baseGrid = new BaseGrid($"{id}-Time", "Time", _chartPresenterContext.DimensionFactory.GetDimension(Constants.Dimension.TIME));
         var values = new DataColumn($"{id}-{valueName}", valueName, _chartPresenterContext.DimensionFactory.NoDimension, baseGrid) {DataInfo = {Origin = ColumnOrigins.CalculationAuxiliary}};
         dataRepository.Add(values);
         return dataRepository;
      }

      public override void Clear()
      {
         base.Clear();
         Chart.RemoveCurve(_markerCurveId);
      }
   }
}