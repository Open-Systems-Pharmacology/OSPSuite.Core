using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
using OSPSuite.Utility.Events;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;

namespace OSPSuite.R.Services
{
   public interface ISensitivityAnalysisRunner
   {
      /// <summary>
      ///    Runs the sensitivity analysis and returns a Core Sensitivity analysis holding the sensitivity analysis results
      /// </summary>
      /// <param name="sensitivityAnalysis">Sensitivity analysis to run</param>
      /// <param name="runOptions">Options to use for the run. If not defined, the default options will be used</param>
      /// <returns>a Core Sensitivity analysis holding the sensitivity analysis results</returns>
      Task<SensitivityAnalysisRunResult> RunAsync(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null);

      /// <summary>
      ///    Runs the sensitivity analysis and returns a Core Sensitivity analysis holding the sensitivity analysis results
      /// </summary>
      /// <param name="sensitivityAnalysis">Sensitivity analysis to run</param>
      /// <param name="runOptions">Options to use for the run. If not defined, the default options will be used</param>
      /// <returns>a Core Sensitivity analysis holding the sensitivity analysis results</returns>
      SensitivityAnalysisRunResult Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null);
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

      public async Task<SensitivityAnalysisRunResult> RunAsync(
         SensitivityAnalysis sensitivityAnalysis,
         SensitivityAnalysisRunOptions runOptions = null)
      {
         var options = runOptions ?? new SensitivityAnalysisRunOptions();

         using (var sensitivityAnalysisEngine = _sensitivityAnalysisEngineFactory.Create())
            try
            {
               initializeProgress(sensitivityAnalysisEngine, options);
               var coreSensitivityAnalysis = _sensitivityAnalysisMapper.MapFrom(sensitivityAnalysis);
               await sensitivityAnalysisEngine.StartAsync(coreSensitivityAnalysis, options);
               return coreSensitivityAnalysis.Results;
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

      public SensitivityAnalysisRunResult Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null) => RunAsync(sensitivityAnalysis, runOptions).Result;

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
      }
   }
}