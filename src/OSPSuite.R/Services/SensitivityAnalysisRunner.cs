using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
using OSPSuite.Utility.Events;

namespace OSPSuite.R.Services
{
   public interface ISensitivityAnalysisRunner
   {
      Task RunAsync(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null);
      void Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null);
   }

   public class SensitivityAnalysisRunner : ISensitivityAnalysisRunner
   {
      private readonly ISensitivityAnalysisEngineFactory _sensitivityAnalysisEngineFactory;
      private readonly ISensitivityAnalysisToCoreSensitivityAnalysisMapper _sensitivityAnalysisMapper;
      private readonly IProgressManager _progressManager;
      private IProgressUpdater _progressUpdater;

      public SensitivityAnalysisRunner(
         ISensitivityAnalysisEngineFactory sensitivityAnalysisEngineFactory,
         ISensitivityAnalysisToCoreSensitivityAnalysisMapper sensitivityAnalysisMapper,
         IProgressManager progressManager
      )
      {
         _sensitivityAnalysisEngineFactory = sensitivityAnalysisEngineFactory;
         _sensitivityAnalysisMapper = sensitivityAnalysisMapper;
         _progressManager = progressManager;
      }

      public async Task RunAsync(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null)
      {
         var options = runOptions ?? new SensitivityAnalysisRunOptions();

         var sensitivityAnalysisEngine = _sensitivityAnalysisEngineFactory.Create();
         try
         {
            initializeProgress(sensitivityAnalysisEngine, options);
            var coreSensitivityAnalysis = _sensitivityAnalysisMapper.MapFrom(sensitivityAnalysis);
            await sensitivityAnalysisEngine.StartAsync(coreSensitivityAnalysis, options);
         }
         finally
         {
            simulationTerminated(sensitivityAnalysisEngine);
         }
      }

      private void initializeProgress(ISensitivityAnalysisEngine sensitivityAnalysisEngine, SensitivityAnalysisRunOptions options)
      {
         sensitivityAnalysisEngine.Terminated += terminated;
         sensitivityAnalysisEngine.SimulationProgress += simulationProgress;
         _progressUpdater = options.ShowProgress ? _progressManager.Create() : new NoneProgressUpdater();
      }

      public void Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null)
      {
         RunAsync(sensitivityAnalysis, runOptions).Wait();
      }

      private void simulationProgress(object sender, MultipleSimulationsProgressEventArgs e)
      {
         _progressUpdater.ReportProgress(e.NumberOfCalculatedSimulation, e.NumberOfSimulations, Messages.CalculationPopulationSimulation(e.NumberOfCalculatedSimulation, e.NumberOfSimulations));
      }

      private void simulationTerminated(ISensitivityAnalysisEngine sensitivityAnalysisEngine)
      {
         terminated(sensitivityAnalysisEngine, new EventArgs());
      }

      private void terminated(object sender, EventArgs e)
      {
         _progressUpdater?.Dispose();
         var engine = sender as SensitivityAnalysisEngine;
         if (engine == null)
            return;

         engine.Terminated -= terminated;
         engine.SimulationProgress -= simulationProgress;
         engine.Dispose();
      }
   }
}