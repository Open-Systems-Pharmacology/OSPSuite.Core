using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Services
{
   public interface ISimulationTask
   {
      IModelCoreSimulation Clone(IModelCoreSimulation simulation);
   }
   public class SimulationTask : ISimulationTask
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public SimulationTask(ICloneManagerForModel cloneManagerForModel)
      {
         _cloneManagerForModel = cloneManagerForModel;
      }
      public IModelCoreSimulation Clone(IModelCoreSimulation simulationToClone)
      {
         var simulation = new ModelCoreSimulation
         {
            Model = _cloneManagerForModel.CloneModel(simulationToClone.Model)
         };

         // Initialize a BuildConfiguration with only SimulationSettings because some of the properties to complete the initialization are required
         // None of the other properties are required to complete the simulation
         var simulationBuildConfiguration = new BuildConfiguration
         {
            SimulationSettings = _cloneManagerForModel.Clone(simulationToClone.SimulationSettings)
         };
         simulation.BuildConfiguration = simulationBuildConfiguration;

         return simulation;
      }
   }
}
