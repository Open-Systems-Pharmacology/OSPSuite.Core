using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
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
      private string _markerCurveId = string.Empty;
      private const string ZERO = "Zero";

      public ParameterIdentificationResidualVsTimeChartPresenter(IParameterIdentificationSingleRunAnalysisView view, ChartPresenterContext chartPresenterContext) :
         base(view, chartPresenterContext, ApplicationIcons.ResidualVsTimeAnalysis, PresenterConstants.PresenterKeys.ParameterIdentificationResidualVsTimeChartPresenter)
      {
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<ParameterIdentificationRunResult> parameterIdentificationResults)
      {
         base.UpdateAnalysisBasedOn(parameterIdentificationResults);
         if (!_parameterIdentification.AllObservedData.Any())
            return;

         addZeroMarkerCurveToChart();

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

      private void addZeroMarkerCurveToChart()
      {
         var markerRepository = createMarkerRepository();
         AddDataRepositoriesToEditor(new[] {markerRepository});
         AddCurvesFor(markerRepository, (column, curve) =>
         {
            curve.UpdateMarkerCurve(ZERO);
            _markerCurveId = curve.Id;
         });
      }

      private DataRepository createMarkerRepository()
      {
         var id = $"{Chart.Id}-{ZERO}";
         var dataRepository = createEmptyRepository(id, ZERO, ZERO);
         dataRepository.BaseGrid.Values = new[] {_parameterIdentification.MinObservedDataTime, _parameterIdentification.MaxObservedDataTime};
         dataRepository.FirstDataColumn().Values = new[] {0f, 0f};
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
         var outputPath = new List<string>(outputResidual.FullOutputPath.ToPathArray());
         //need to create a unique path containing the observed data name. Since we want to keep the entity name and simulation name, we have to insert before the name
         if (outputPath.Count > 0)
            outputPath.Insert(outputPath.Count - 1, outputResidual.ObservedDataName);

         scatterColumn.QuantityInfo.Path = outputPath;
         return dataRepository;
      }

      private DataRepository createEmptyRepository(string id, string name, string valueName)
      {
         var dataRepository = new DataRepository(id) {Name = name};
         var baseGrid = new BaseGrid($"{id}-Time", "Time", _chartPresenterContext.DimensionFactory.Dimension(Constants.Dimension.TIME));
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