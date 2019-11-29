using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;

namespace OSPSuite.Core.Services
{
   public interface ISimulationAnalyzer
   {
      /// <summary>
      /// Returns the path of all parameters really used in the <paramref name="modelCoreSimulation"/>. 
      /// </summary>
      IReadOnlyList<string> AllPathOfParametersUsedInSimulation(IModelCoreSimulation modelCoreSimulation);
   }

   public class SimulationAnalyzer : SimModelManagerBase, ISimulationAnalyzer
   {
      public SimulationAnalyzer(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory) : base(simModelExporter, simModelSimulationFactory)
      {
      }

      public IReadOnlyList<string> AllPathOfParametersUsedInSimulation(IModelCoreSimulation modelCoreSimulation)
      {
         var simulationExport = CreateSimulationExport(modelCoreSimulation, SimModelExportMode.Optimized);
         var simulation = CreateSimulation(simulationExport, o => o.IdentifyUsedParameters = true);
         FinalizeSimulation(simulation);
         var allUsedParameters = simulation.ParameterProperties.Where(x => x.IsUsedInSimulation);
         return allUsedParameters.Select(x => x.Path).ToList();
      }
   }
}