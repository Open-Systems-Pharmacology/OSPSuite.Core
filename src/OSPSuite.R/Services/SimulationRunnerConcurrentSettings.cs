using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using System.Collections.Generic;
using SimulationRunOptions = OSPSuite.R.Domain.SimulationRunOptions;

namespace OSPSuite.R.Services
{
   public class SimulationWithPopulationConcurrentSetting
   {
      public IModelCoreSimulation Simulation { get; private set; }
      public IndividualValuesCache Population { get; private set; }

      public SimulationWithPopulationConcurrentSetting(IModelCoreSimulation simulation, IndividualValuesCache population)
      {
         Simulation = simulation;
         Population = population;
      }
   }

   /// <summary>
   /// Settings to run simulations concurrently
   /// </summary>
   public class SimulationRunnerConcurrentSettings
   {
      private IList<SimulationWithPopulationConcurrentSetting> _simulationWithPopulation = new List<SimulationWithPopulationConcurrentSetting>();

      /// <summary>
      /// List of Simulation/Population pairs
      /// </summary>
      public IReadOnlyList<SimulationWithPopulationConcurrentSetting> SimulationWithPopulation
      {
         get {
            return (IReadOnlyList<SimulationWithPopulationConcurrentSetting>)_simulationWithPopulation;
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
         _simulationWithPopulation.Add(new SimulationWithPopulationConcurrentSetting(simulation, population));
      }

      /// <summary>
      /// Adds a simulation to run without population
      /// </summary>
      /// <param name="simulation"></param>
      public void Add(IModelCoreSimulation simulation)
      {
         _simulationWithPopulation.Add(new SimulationWithPopulationConcurrentSetting(simulation, null));
      }
   }
}
