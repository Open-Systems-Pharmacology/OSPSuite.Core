using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.R.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using CoreSensitivityAnalysis = OSPSuite.Core.Domain.SensitivityAnalyses.SensitivityAnalysis;

namespace OSPSuite.R.Mapper
{
   public interface ISensitivityAnalysisToCoreSensitivityAnalysisMapper : IMapper<SensitivityAnalysis, CoreSensitivityAnalysis>
   {
   }

   public class SensitivityAnalysisToCoreSensitivityAnalysisMapper : ISensitivityAnalysisToCoreSensitivityAnalysisMapper
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISimulationAnalyzer _simulationAnalyzer;
      private readonly IParameterAnalysableParameterSelector _parameterSelector;
      private readonly IContainerTask _containerTask;

      public SensitivityAnalysisToCoreSensitivityAnalysisMapper(
         ISensitivityAnalysisTask sensitivityAnalysisTask,
         ISimulationAnalyzer simulationAnalyzer,
         IParameterAnalysableParameterSelector parameterSelector,
         IContainerTask containerTask
      )
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _simulationAnalyzer = simulationAnalyzer;
         _parameterSelector = parameterSelector;
         _containerTask = containerTask;
      }

      public CoreSensitivityAnalysis MapFrom(SensitivityAnalysis sensitivityAnalysis)
      {
         var simulation = sensitivityAnalysis.Simulation;
         var coreSensitivityAnalysis = _sensitivityAnalysisTask.CreateSensitivityAnalysisFor(simulation);
         var parametersToVary = parameterPathsToVaryFrom(sensitivityAnalysis).Select(x => new ParameterSelection(simulation, x)).ToList();

         _sensitivityAnalysisTask.AddParametersTo(coreSensitivityAnalysis, parametersToVary);

         coreSensitivityAnalysis.AllSensitivityParameters.Each(x =>
         {
            x.NumberOfStepsParameter.Value = sensitivityAnalysis.NumberOfSteps;
            x.VariationRangeParameter.Value = sensitivityAnalysis.VariationRange;
         });


         return coreSensitivityAnalysis;
      }

      private IReadOnlyList<string> parameterPathsToVaryFrom(SensitivityAnalysis sensitivityAnalysis)
      {
         if (sensitivityAnalysis.ParameterPaths.Any())
            return sensitivityAnalysis.ParameterPaths;

         var simulation = sensitivityAnalysis.Simulation;
         var constantParametersCache = _containerTask.CacheAllChildrenSatisfying<IParameter>(simulation.Model.Root, x => _parameterSelector.CanUseParameter(x) && x.IsConstantParameter());
         var allUsedParameterPaths = _simulationAnalyzer.AllPathOfParametersUsedInSimulation(sensitivityAnalysis.Simulation);

         return allUsedParameterPaths.Select(x => new
            {
               Parameter = constantParametersCache[x],
               Path = x
            })
            .Where(x => x.Parameter != null)
            .Select(x => x.Path).ToList();
      }
   }
}