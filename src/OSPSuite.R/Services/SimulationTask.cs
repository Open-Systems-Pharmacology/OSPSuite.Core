using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.R.Services
{
   public interface ISimulationTask
   {
      /// <summary>
      ///    Returns a clone of the simulation that can be used during batch run.
      ///    This simulation will contain the minimal information required to run. Configuration settings (e.g. some building
      ///    blocks) will not be copied over
      /// </summary>
      /// <param name="simulation">Simulation to clone</param>
      IModelCoreSimulation CloneForBatchRun(IModelCoreSimulation simulation);
   }

   public class SimulationTask : ISimulationTask
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;

      public SimulationTask(ICloneManagerForModel cloneManagerForModel, ISimulationPersistableUpdater simulationPersistableUpdater)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _simulationPersistableUpdater = simulationPersistableUpdater;
      }

      public IModelCoreSimulation CloneForBatchRun(IModelCoreSimulation simulationToClone)
      {
         var simulation = new ModelCoreSimulation
         {
            Model = _cloneManagerForModel.CloneModel(simulationToClone.Model),
            // None of the other properties are required to complete the simulation
            // Initialize a BuildConfiguration with only SimulationSettings because some of the properties to complete the initialization are required
            Configuration = new SimulationConfiguration
            {
               SimulationSettings = _cloneManagerForModel.Clone(simulationToClone.Settings)
            }
         };


         //Once we prepare a simulation for a batch, we are assuming that it won't be changed by the caller.
         //We update the persistable at this stage to avoid any possible thread problems
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);

         return simulation;
      }
   }
}