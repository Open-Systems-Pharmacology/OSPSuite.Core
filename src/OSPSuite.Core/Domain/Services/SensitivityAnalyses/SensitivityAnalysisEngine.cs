using System;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisEngine : IDisposable
   {
      Task StartAsync(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions, CancellationToken cancellationToken = default);

      /// <summary>
      ///    Progress event returns the percent representing the progress of a simulation
      /// </summary>
      event EventHandler<MultipleSimulationsProgressEventArgs> SimulationProgress;

      /// <summary>
      ///    Event raised when simulation is terminated (either after normal termination or cancel)
      /// </summary>
      event EventHandler Terminated;
   }

   public class SensitivityAnalysisEngine : ISensitivityAnalysisEngine
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly ISensitivityAnalysisVariationDataCreator _sensitivityAnalysisVariationDataCreator;
      private readonly IPopulationRunner _populationRunner;
      private readonly ISimulationToModelCoreSimulationMapper _modelCoreSimulationMapper;
      private readonly ISensitivityAnalysisRunResultCalculator _runResultCalculator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private SensitivityAnalysis _sensitivityAnalysis;
      public event EventHandler<MultipleSimulationsProgressEventArgs> SimulationProgress = delegate { };
      public event EventHandler Terminated = delegate { };

      public SensitivityAnalysisEngine(
         IEventPublisher eventPublisher, 
         ISensitivityAnalysisVariationDataCreator sensitivityAnalysisVariationDataCreator,
         IPopulationRunner populationRunner, 
         ISimulationToModelCoreSimulationMapper modelCoreSimulationMapper,
         ISensitivityAnalysisRunResultCalculator runResultCalculator, 
         ISimulationPersistableUpdater simulationPersistableUpdater)
      {
         _eventPublisher = eventPublisher;
         _sensitivityAnalysisVariationDataCreator = sensitivityAnalysisVariationDataCreator;
         _populationRunner = populationRunner;
         _modelCoreSimulationMapper = modelCoreSimulationMapper;
         _runResultCalculator = runResultCalculator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _populationRunner.Terminated += terminated;
         _populationRunner.SimulationProgress += simulationProgress;
      }

      public async Task StartAsync(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions, CancellationToken cancellationToken = default)
      {
         _sensitivityAnalysis = sensitivityAnalysis;
         _eventPublisher.PublishEvent(new SensitivityAnalysisStartedEvent(sensitivityAnalysis));

         try
         {
            var modelCoreSimulation = _modelCoreSimulationMapper.MapFrom(sensitivityAnalysis.Simulation, shouldCloneModel: true);
            _simulationPersistableUpdater.UpdateSimulationPersistable(modelCoreSimulation);
            var variationData = _sensitivityAnalysisVariationDataCreator.CreateForRun(sensitivityAnalysis);
            var runResults = await _populationRunner.RunPopulationAsync(modelCoreSimulation, runOptions,  variationData.ToDataTable(), cancellationToken: cancellationToken);
            sensitivityAnalysis.Results = await calculateSensitivityBasedOn(sensitivityAnalysis, variationData, runResults, runOptions);
            _eventPublisher.PublishEvent(new SensitivityAnalysisResultsUpdatedEvent(sensitivityAnalysis));
         }
         finally
         {
            _eventPublisher.PublishEvent(new SensitivityAnalysisTerminatedEvent(sensitivityAnalysis));
            _sensitivityAnalysis = null;
         }
      }

      private Task<SensitivityAnalysisRunResult> calculateSensitivityBasedOn(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, PopulationRunResults runResults, SensitivityAnalysisRunOptions sensitivityAnalysisRunOptions)
      {
         return Task.Run(() => _runResultCalculator.CreateFor(sensitivityAnalysis, variationData, runResults.Results, runResults.Errors, sensitivityAnalysisRunOptions.ReturnOutputValues));
      }

      private void simulationProgress(object sender, MultipleSimulationsProgressEventArgs eventArgs)
      {
         _eventPublisher.PublishEvent(new SensitivityAnalysisProgressEvent(_sensitivityAnalysis, eventArgs.NumberOfCalculatedSimulation, eventArgs.NumberOfSimulations));
         SimulationProgress(this, eventArgs);
      }

      private void terminated(object sender, EventArgs e)
      {
         _populationRunner.Terminated -= terminated;
         _populationRunner.SimulationProgress -= simulationProgress;
         Terminated(this, e);
      }

      protected virtual void Cleanup()
      {
         _sensitivityAnalysis = null;
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~SensitivityAnalysisEngine()
      {
         Cleanup();
      }

      #endregion
   }
}