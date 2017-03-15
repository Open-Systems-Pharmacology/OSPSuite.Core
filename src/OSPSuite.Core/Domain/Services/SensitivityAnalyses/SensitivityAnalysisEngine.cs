using System;
using System.Threading.Tasks;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisEngine : IDisposable
   {
      Task StartAsync(SensitivityAnalysis sensitivityAnalysis);
      void Stop();
   }

   public class SensitivityAnalysisEngine : ISensitivityAnalysisEngine
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly ISensitivyAnalysisVariationDataCreator _sensitivyAnalysisVariationDataCreator;
      private readonly IPopulationRunner _populationRunner;
      private readonly ICoreUserSettings _userSettings;
      private readonly ISimulationToModelCoreSimulationMapper _modelCoreSimulationMapper;
      private readonly ISensitivityAnalysisRunResultCalculator _runResultCalculator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private SensitivityAnalysis _sensitivityAnalysis;

      public SensitivityAnalysisEngine(IEventPublisher eventPublisher, ISensitivyAnalysisVariationDataCreator sensitivyAnalysisVariationDataCreator,
         IPopulationRunner populationRunner, ICoreUserSettings userSettings, ISimulationToModelCoreSimulationMapper modelCoreSimulationMapper,
         ISensitivityAnalysisRunResultCalculator runResultCalculator, ISimulationPersistableUpdater simulationPersistableUpdater)
      {
         _eventPublisher = eventPublisher;
         _sensitivyAnalysisVariationDataCreator = sensitivyAnalysisVariationDataCreator;
         _populationRunner = populationRunner;
         _userSettings = userSettings;
         _modelCoreSimulationMapper = modelCoreSimulationMapper;
         _runResultCalculator = runResultCalculator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _populationRunner.Terminated += terminated;
         _populationRunner.SimulationProgress += simulationProgress;
      }

      public async Task StartAsync(SensitivityAnalysis sensitivityAnalysis)
      {
         _sensitivityAnalysis = sensitivityAnalysis;
         _eventPublisher.PublishEvent(new SensitivityAnalysisStartedEvent(sensitivityAnalysis));
         _populationRunner.NumberOfCoresToUse = _userSettings.MaximumNumberOfCoresToUse;

         try
         {
            var modelCoreSimulation = _modelCoreSimulationMapper.MapFrom(sensitivityAnalysis.Simulation, shouldCloneModel: true);
            _simulationPersistableUpdater.UpdateSimulationPersistable(modelCoreSimulation);
            var variationData = _sensitivyAnalysisVariationDataCreator.CreateForRun(sensitivityAnalysis);
            var runResults = await _populationRunner.RunPopulationAsync(modelCoreSimulation, variationData.ToDataTable());
            sensitivityAnalysis.Results = await calculateSensitivityBasedOn(sensitivityAnalysis, variationData, runResults);
            _eventPublisher.PublishEvent(new SensitivityAnalysisResultsUpdatedEvent(sensitivityAnalysis));
         }
         finally
         {
            _eventPublisher.PublishEvent(new SensitivityAnalysisTerminatedEvent(sensitivityAnalysis));
            _sensitivityAnalysis = null;
         }
      }

      private Task<SensitivityAnalysisRunResult> calculateSensitivityBasedOn(SensitivityAnalysis sensitivityAnalysis, VariationData variationData, PopulationRunResults runResults)
      {
         return Task.Run(() => _runResultCalculator.CreateFor(sensitivityAnalysis, variationData, runResults.Results));
      }

      private void simulationProgress(object sender, PopulationSimulationProgressEventArgs eventArgs)
      {
         _eventPublisher.PublishEvent(new SensitivityAnalysisProgressEvent(_sensitivityAnalysis, eventArgs.NumberOfCalculatedSimulation, eventArgs.NumberOfSimulations));
      }

      private void terminated(object sender, EventArgs e)
      {
         _populationRunner.Terminated -= terminated;
         _populationRunner.SimulationProgress -= simulationProgress;
      }

      public void Stop()
      {
         _populationRunner.StopSimulation();
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