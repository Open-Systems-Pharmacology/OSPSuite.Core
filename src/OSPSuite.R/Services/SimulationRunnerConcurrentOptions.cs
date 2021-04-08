using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using System.Collections.Generic;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   /// <summary>
   /// Options to run simulations concurrently
   /// </summary>
   public class SimulationRunnerConcurrentOptions
   {
      private List<IModelCoreSimulation> _simulations = new List<IModelCoreSimulation>();
      /// <summary>
      /// List of Simulation/Population pairs
      /// </summary>
      public IReadOnlyList<IModelCoreSimulation> Simulations { get => _simulations; }
      /// <summary>
      /// General simulation options
      /// </summary>
      public SimulationRunOptions simulationRunOptions { get; set; }

      public void AddSimulation(IModelCoreSimulation simulation)
      {
         _simulations.Add(simulation);
      }
   }
}
