using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   public interface IOutputSelectionUpdater
   {
      /// <summary>
      ///    Removes all outputs of <paramref name="simulation" /> that are not in <paramref name="usedOutputsForSimulation" />
      /// </summary>
      void UpdateOutputsIn(IModelCoreSimulation simulation, IEnumerable<QuantitySelection> usedOutputsForSimulation);
   }

   public class OutputSelectionUpdater : IOutputSelectionUpdater
   {
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;

      public OutputSelectionUpdater(ISimulationPersistableUpdater simulationPersistableUpdater)
      {
         _simulationPersistableUpdater = simulationPersistableUpdater;
      }

      public void UpdateOutputsIn(IModelCoreSimulation simulation, IEnumerable<QuantitySelection> usedOutputsForSimulation)
      {
         var outputSelection = simulation.BuildConfiguration.SimulationSettings.OutputSelections;
         var usedOutputsList = usedOutputsForSimulation.ToList();

         //Remove the one mapped in the simulation that are not used 
         foreach (var simulationOutput in outputSelection.AllOutputs.ToList().Where(output => !usedOutputsList.Contains(output)))
         {
            outputSelection.RemoveOutput(simulationOutput);
         }

         //Adds the one that are missing
         foreach (var usedOutput in usedOutputsList.Where(output => !outputSelection.AllOutputs.Contains(output)))
         {
            outputSelection.AddOutput(usedOutput);
         }

         //Now that the settings were updated, update the persistable flag
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
      }
   }
}