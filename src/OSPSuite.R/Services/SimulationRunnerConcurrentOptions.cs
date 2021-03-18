using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using System.Collections.Generic;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public class SimulationWithPopulationConcurrentOptions
   {
      public IModelCoreSimulation Simulation { get; private set; }
      public IndividualValuesCache Population { get; private set; }

      public SimulationWithPopulationConcurrentOptions(IModelCoreSimulation simulation, IndividualValuesCache population)
      {
         Simulation = simulation;
         Population = population;
      }
   }

   /// <summary>
   /// Options to run simulations concurrently
   /// </summary>
   public class SimulationRunnerConcurrentOptions
   {
      private IList<SimulationWithPopulationConcurrentOptions> _simulationsWithPopulations = new List<SimulationWithPopulationConcurrentOptions>();

      /// <summary>
      /// List of Simulation/Population pairs
      /// </summary>
      public IReadOnlyList<SimulationWithPopulationConcurrentOptions> SimulationsWithPopulations
      {
         get {
            return (IReadOnlyList<SimulationWithPopulationConcurrentOptions>)_simulationsWithPopulations;
         }
      }
      /// <summary>
      /// General simulation options
      /// </summary>
      public SimulationRunOptions simulationRunOptions { get; set; }

      /// <summary>
      /// Adds a new pair
      /// </summary>
      /// <param name="simulation">Simulation object</param>
      /// <param name="population">Population definition</param>
      public void Add(IModelCoreSimulation simulation, IndividualValuesCache population)
      {
         _simulationsWithPopulations.Add(new SimulationWithPopulationConcurrentOptions(simulation, population));
      }

      /// <summary>
      /// Adds a simulation to run without population
      /// </summary>
      /// <param name="simulation"></param>
      public void Add(IModelCoreSimulation simulation)
      {
         _simulationsWithPopulations.Add(new SimulationWithPopulationConcurrentOptions(simulation, null));
      }
   }
}
