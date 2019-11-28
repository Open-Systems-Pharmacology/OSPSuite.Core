using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
using OSPSuite.Utility;

namespace OSPSuite.R.Services
{
   public interface ISensitivityAnalysisRunner
   {
      Task RunSimulationAsync(SensitivityAnalysis sensitivityAnalysis, RunOptions runOptions = null);
      void RunSimulation(SensitivityAnalysis sensitivityAnalysis, RunOptions runOptions = null);
   }

   public class SensitivityAnalysisRunner : ISensitivityAnalysisRunner
   {
      private readonly ISensitivityAnalysisEngineFactory _sensitivityAnalysisEngineFactory;
      private readonly ISensitivityAnalysisToCoreSensitivityAnalysisMapper _sensitivityAnalysisMapper;

      public SensitivityAnalysisRunner(ISensitivityAnalysisEngineFactory sensitivityAnalysisEngineFactory, ISensitivityAnalysisToCoreSensitivityAnalysisMapper sensitivityAnalysisMapper)
      {
         _sensitivityAnalysisEngineFactory = sensitivityAnalysisEngineFactory;
         _sensitivityAnalysisMapper = sensitivityAnalysisMapper;
      }

      public async Task RunSimulationAsync(SensitivityAnalysis sensitivityAnalysis, RunOptions runOptions = null)
      {
         var options = runOptions ?? new RunOptions();

         using (var sensitivityAnalysisEngine = _sensitivityAnalysisEngineFactory.Create())
         {
            var begin = SystemTime.UtcNow();
            var coreSensitivityAnalysis = _sensitivityAnalysisMapper.MapFrom(sensitivityAnalysis);
            await sensitivityAnalysisEngine.StartAsync(coreSensitivityAnalysis, options);
            var end = SystemTime.UtcNow();
            var timeSpent = end - begin;
         }
      }

      public void RunSimulation(SensitivityAnalysis sensitivityAnalysis, RunOptions runOptions = null)
      {
         RunSimulationAsync(sensitivityAnalysis, runOptions).Wait();
      }
   }
}