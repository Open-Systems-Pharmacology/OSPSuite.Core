using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;
using Constants = OSPSuite.Core.Domain.Constants;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationResidualVsTimeChartPresenter : IChartPresenter<SimulationResidualVsTimeChart>,
      ISimulationAnalysisPresenter
   {
      ISimulationRunAnalysisView View { get; }
   }

   public class SimulationResidualVsTimeChartPresenter : SimulationRunAnalysisPresenter<SimulationResidualVsTimeChart>,
      ISimulationResidualVsTimeChartPresenter
   {
      private readonly IObservedDataRepository _observedDataRepository;
      private readonly IResidualCalculator _residualCalculator;
      private IReadOnlyCollection<OutputResiduals> _allOutputResiduals;
      private string _markerCurveId = string.Empty;
      private const string ZERO = "Zero";

      public SimulationResidualVsTimeChartPresenter(ISimulationRunAnalysisView view, ChartPresenterContext chartPresenterContext, 
          IObservedDataRepository observedDataRepository, IResidualCalculatorFactory residualCalculatorFactory) 
         : base(view, chartPresenterContext, ApplicationIcons.PredictedVsObservedAnalysis, PresenterConstants.PresenterKeys.SimulationPredictedVsActualChartPresenter)
      {
         _observedDataRepository = observedDataRepository;
         _residualCalculator = residualCalculatorFactory.CreateFor(new ParameterIdentificationConfiguration());
      }

      protected override void UpdateAnalysisBasedOn(IReadOnlyList<IndividualResults> simulationResults)
      {
         base.UpdateAnalysisBasedOn(simulationResults);
         var simulationResidual = _residualCalculator.CalculateForSimulation(_simulation.ResultRepository, _simulation.OutputMappings.All);
         _allOutputResiduals = simulationResidual.AllOutputResiduals;
         if (!getAllAvailableObservedData().Any())
            return;

         addZeroMarkerCurveToChart();//create a service for this

         if (ChartIsBeingCreated)
         {
            Chart.AxisBy(AxisTypes.Y).Caption = Captions.ParameterIdentification.Residuals;
            Chart.AxisBy(AxisTypes.Y).Scaling = Scalings.Linear;
         }

         UpdateChartFromTemplate();
         View.SetTotalError(simulationResidual.TotalError);
      }

      protected override void AddRunResultToChart()
      {
         _allOutputResiduals.GroupBy(x => x.FullOutputPath).Each(addOutputToScatter);
      }

      protected void AddResultRepositoryToEditor(DataRepository dataRepository)
      {
         AddResultRepositoriesToEditor(new[] { dataRepository });
      }

      private void addOutputToScatter(IGrouping<string, OutputResiduals> outputMappingsByOutput)
      {
         var fullOutputPath = outputMappingsByOutput.Key;
         bool shouldShowInLegend = true;
         foreach (var outputMapping in outputMappingsByOutput)
         {
            var dataRepository = getOrCreateScatterDataRepositoryFor(outputMapping);
            AddResultRepositoryToEditor(dataRepository);
            var visibleInLegend = shouldShowInLegend;

            AddCurvesFor(dataRepository, (column, curve) =>
            {
               SelectColorForPath(fullOutputPath);
               UpdateColorForPath(curve, fullOutputPath);
               curve.Name = fullOutputPath;
               curve.Description = "description";
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
         AddDataRepositoriesToEditor(new[] { markerRepository });
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
         dataRepository.BaseGrid.Values = new[] { minObservedDataTime(), MaxObservedDataTime() };
         dataRepository.FirstDataColumn().Values = new[] { 0f, 0f };
         return dataRepository;
      }

      private float minObservedDataTime()
      { 
         return getAllAvailableObservedData().Select(x => x.BaseGrid.Values.First()).Min();
      }

      private float MaxObservedDataTime()
      {
         return getAllAvailableObservedData().Select(x => x.BaseGrid.Values.Last()).Max();
      }

      private DataRepository getOrCreateScatterDataRepositoryFor( OutputResiduals outputResidual)
      {
         var repositoryName = "Simulation Results";
         var id = $"{Chart.Id}-{outputResidual.FullOutputPath}-{outputResidual.ObservedData.Id}";

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

      private IEnumerable<DataRepository> getAllAvailableObservedData()
      {
         return _observedDataRepository.AllObservedDataUsedBy(_simulation)
            .Distinct()
            .OrderBy(x => x.Name);
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
         var dataRepository = new DataRepository(id) { Name = name };
         var baseGrid = new BaseGrid($"{id}-Time", "Time", _chartPresenterContext.DimensionFactory.Dimension(Constants.Dimension.TIME));
         var values = new DataColumn($"{id}-{valueName}", valueName, _chartPresenterContext.DimensionFactory.NoDimension, baseGrid) { DataInfo = { Origin = ColumnOrigins.CalculationAuxiliary } };
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