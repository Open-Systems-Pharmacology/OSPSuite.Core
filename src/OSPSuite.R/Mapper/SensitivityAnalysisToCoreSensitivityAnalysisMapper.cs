using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.R.Domain;
using OSPSuite.R.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using CoreSensitivityAnalysis = OSPSuite.Core.Domain.SensitivityAnalyses.SensitivityAnalysis;
using IContainerTask = OSPSuite.Core.Domain.Services.IContainerTask;

namespace OSPSuite.R.Mapper
{
   public interface ISensitivityAnalysisToCoreSensitivityAnalysisMapper : IMapper<SensitivityAnalysis, CoreSensitivityAnalysis>
   {
   }

   public class SensitivityAnalysisToCoreSensitivityAnalysisMapper : ISensitivityAnalysisToCoreSensitivityAnalysisMapper
   {
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ISimulationAnalyzer _simulationAnalyzer;
      private readonly IParameterAnalysableParameterSelector _parameterSelector;
      private readonly IContainerTask _containerTask;

      public SensitivityAnalysisToCoreSensitivityAnalysisMapper(
         ISensitivityAnalysisTask sensitivityAnalysisTask,
         IEntityPathResolver entityPathResolver,
         ISimulationAnalyzer simulationAnalyzer,
         IParameterAnalysableParameterSelector parameterSelector,
         IContainerTask containerTask
         )
      {
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _entityPathResolver = entityPathResolver;
         _simulationAnalyzer = simulationAnalyzer;
         _parameterSelector = parameterSelector;
         _containerTask = containerTask;
      }

      public CoreSensitivityAnalysis MapFrom(SensitivityAnalysis sensitivityAnalysis)
      {
         var simulation = sensitivityAnalysis.Simulation;
         var coreSensitivityAnalysis = _sensitivityAnalysisTask.CreateSensitivityAnalysisFor(simulation);
         var parametersToVary = parametersToVaryFrom(sensitivityAnalysis).Select(x => new ParameterSelection(simulation, _entityPathResolver.PathFor(x))).ToList();

         _sensitivityAnalysisTask.AddParametersTo(coreSensitivityAnalysis, parametersToVary);

         coreSensitivityAnalysis.AllSensitivityParameters.Each(x =>
         {
            x.NumberOfStepsParameter.Value = sensitivityAnalysis.NumberOfSteps;
            x.VariationRangeParameter.Value = sensitivityAnalysis.VariationRange;
         });

         return coreSensitivityAnalysis;
      }

      private IReadOnlyList<IParameter> parametersToVaryFrom(SensitivityAnalysis sensitivityAnalysis)
      {
         var parametersToVary = new List<IParameter>(sensitivityAnalysis.Parameters);
         if (parametersToVary.Any())
            return parametersToVary;

         var simulation = sensitivityAnalysis.Simulation;
         var constantParametersCache = _containerTask.CacheAllChildrenSatisfying<IParameter>(simulation.Model.Root, x => _parameterSelector.CanUseParameter(x) && x.IsConstantParameter());
         var allUsedParameterPaths = _simulationAnalyzer.AllPathOfParametersUsedInSimulation(sensitivityAnalysis.Simulation);

         return allUsedParameterPaths.Select(x => constantParametersCache[x]).Where(x=>x!=null).ToList();
      }

   }
}