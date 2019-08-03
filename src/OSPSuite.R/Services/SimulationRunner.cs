using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.R.Services
{
   public interface ISimulationRunner
   {
      Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
      SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null);
   }

   public class SimulationRunner : ISimulationRunner
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly RandomGenerator _randomGenerator;

      public SimulationRunner(IEntitiesInSimulationRetriever entitiesInSimulationRetriever)
      {
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
         _randomGenerator = new RandomGenerator();
      }

      public SimulationResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, simulationRunOptions).Result;
      }

      public Task<SimulationResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         var simulationResults = new SimulationResults {individualResultsFrom(simulation, 1)};

         return Task.FromResult<SimulationResults>(simulationResults);
      }

      private IndividualResults individualResultsFrom(IModelCoreSimulation simulation, int individualId)
      {
         var results = new IndividualResults {IndividualId = individualId};
         var simulationTimesLength = 100;
         var allQuantities = _entitiesInSimulationRetriever.QuantitiesFrom(simulation, x=>x.Persistable);
         
         foreach (var quantityAndPath in allQuantities.KeyValues)
         {
            //Add quantity name and remove simulation name
            var quantityPath = quantityAndPath.Key.ToPathArray().ToList();
            quantityPath.Remove(simulation.Name);
            results.Add(quantityValuesFor(quantityPath.ToPathString(), simulationTimesLength));
         }


         results.Time = quantityValuesFor(Constants.TIME, Enumerable.Range(0, 100).Select(x => x * 10.0).ToArray());
         return results;
      }

      private QuantityValues quantityValuesFor(string quantityPath, int expectedLength)
      {
         return quantityValuesFor(quantityPath, generateRandomValues(expectedLength));
      }

      private QuantityValues quantityValuesFor(string quantityPath, double[] values)
      {
         return new QuantityValues
         {
            QuantityPath = quantityPath,
            Values = values.ToFloatArray()
         };
      }

      private double[] generateRandomValues(int numberOfValues)
      {
         var randomValues = new List<double>();
         for (int i = 0; i < numberOfValues; i++)
         {
            randomValues.Add(_randomGenerator.NextDouble());
         }

         return randomValues.ToArray();
      }
   }
}