using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationResidualVsTimeChartPresenter : IChartPresenter<SimulationResidualVsTimeChart>,
      ISimulationAnalysisPresenter
   {
      ISimulationVsObservedDataView View { get; }
   }

   public class SimulationResidualVsTimeChartPresenter : SimulationVsObservedDataChartPresenter<SimulationResidualVsTimeChart>,
      ISimulationResidualVsTimeChartPresenter
   {
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IResidualsVsTimeChartService _residualsVsTimeChartService;
      private readonly IResidualCalculator _residualCalculator;
      private DataRepository _zeroRepository = new DataRepository();
      private IReadOnlyCollection<OutputResiduals> _allOutputResiduals;

      public SimulationResidualVsTimeChartPresenter(ISimulationVsObservedDataView view, ChartPresenterContext chartPresenterContext,
         IObservedDataRepository observedDataRepository, IResidualCalculatorFactory residualCalculatorFactory,
         IResidualsVsTimeChartService residualsVsTimeChartService)
         : base(view, chartPresenterContext, ApplicationIcons.ResidualVsTimeAnalysis,
            PresenterConstants.PresenterKeys.SimulationPredictedVsObservedChartPresenter)
      {
         _observedDataRepository = observedDataRepository;
         _residualCalculator = residualCalculatorFactory.CreateFor(new ParameterIdentificationConfiguration());
         _residualsVsTimeChartService = residualsVsTimeChartService;
      }

      protected override void UpdateAnalysis()
      {
         addRunResultToChart();

         var availableObservedData = getAllAvailableObservedData();
         if (!availableObservedData.Any())
            return;

         _zeroRepository = _residualsVsTimeChartService.AddZeroMarkerCurveToChart(Chart, minObservedDataTime(availableObservedData), maxObservedDataTime(availableObservedData));
         AddDataRepositoriesToEditor(new[] { _zeroRepository });

         _residualsVsTimeChartService.ConfigureChartAxis(Chart);

         UpdateChartFromTemplate();
      }

      private void addRunResultToChart()
      {
         var simulationResidual = _residualCalculator.Calculate(_simulation.ResultsDataRepository, _simulation.OutputMappings.All);
         _allOutputResiduals = simulationResidual.AllOutputResiduals;
         _allOutputResiduals.GroupBy(x => x.FullOutputPath).Each(addOutputToScatter);
         View.SetTotalError(simulationResidual.TotalError);
      }

      private void addOutputToScatter(IGrouping<string, OutputResiduals> outputMappingsByOutput)
      {
         var fullOutputPath = outputMappingsByOutput.Key;
         var shouldShowInLegend = true;
         foreach (var outputMapping in outputMappingsByOutput)
         {
            var dataRepository = getOrCreateScatterDataRepositoryFor(outputMapping);
            AddResultRepositoryToEditor(dataRepository);
            var visibleInLegend = shouldShowInLegend;

            AddCurvesFor(dataRepository, (column, curve) =>
            {
               SelectColorForPath(fullOutputPath);
               UpdateColorForPath(curve, fullOutputPath);
               curve.Name = NameForColumn(column);
               curve.Description = outputMapping.ObservedDataName;
               curve.VisibleInLegend = visibleInLegend;
            });

            shouldShowInLegend = false;
         }
      }

      private float minObservedDataTime(IReadOnlyList<DataRepository> availableObservedData) =>
         availableObservedData.Select(x => x.BaseGrid.Values.First()).Min();

      private float maxObservedDataTime(IReadOnlyList<DataRepository> availableObservedData) =>
         availableObservedData.Select(x => x.BaseGrid.Values.Last()).Max();

      private DataRepository getOrCreateScatterDataRepositoryFor(OutputResiduals outputResidual) =>
         _residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(Chart, outputResidual);

      private IReadOnlyList<DataRepository> getAllAvailableObservedData()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name).ToList();
      }

      public override void Clear()
      {
         base.Clear();
         Chart.RemoveCurvesForDataRepository(_zeroRepository);
      }
   }
}