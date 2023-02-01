using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Services
{
   public interface IOutputMappingMatchingTask
   {
      /// <summary>
      ///    Adds an output mapping to the <paramref name="simulation"/> for the <paramref name="observedData"/> in the case
      ///   that an automatic matching output can be found, according to the DataRepository metaData.
      /// </summary>
      void AddMatchingOutputMapping(DataRepository observedData, ISimulation simulation);

      /// <summary>
      ///    Returns true if the <paramref name="observedData"/> matches the output path according to the
      ///   DataRepository metaData.
      /// </summary>
      bool ObservedDataMatchesOutput(DataRepository observedData, string outputPath);

      /// <summary>
      ///    Returns the default scaling for the <paramref name="output" />
      /// </summary>
      Scalings DefaultScalingFor(IQuantity output);
   }

   public class OutputMappingMatchingTask : IOutputMappingMatchingTask
   {
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      private readonly IEventPublisher _eventPublisher;

      public OutputMappingMatchingTask (IEntitiesInSimulationRetriever entitiesInSimulationRetriever, IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public void AddMatchingOutputMapping(DataRepository observedData, ISimulation simulation)
      {
         var newOutputMapping = mapMatchingOutput(observedData, simulation);

         if (newOutputMapping.Output == null) 
            return;
         
         simulation.OutputMappings.Add(newOutputMapping);
         _eventPublisher.PublishEvent(new SimulationOutputMappingsChangedEvent(simulation));
      }
      private OutputMapping mapMatchingOutput(DataRepository observedData, ISimulation simulation)
      {
         var newOutputMapping = new OutputMapping();
         var pathCache = _entitiesInSimulationRetriever.OutputsFrom(simulation);
         var matchingOutputPath = pathCache.Keys.FirstOrDefault(x => ObservedDataMatchesOutput(observedData, x));

         if (matchingOutputPath == null)
            return newOutputMapping;

         var matchingOutput = pathCache[matchingOutputPath];

         newOutputMapping.OutputSelection =
            new SimulationQuantitySelection(simulation, new QuantitySelection(matchingOutputPath, matchingOutput.QuantityType));
         newOutputMapping.WeightedObservedData = new WeightedObservedData(observedData);
         newOutputMapping.Scaling = DefaultScalingFor(matchingOutput);
         return newOutputMapping;
      }

      public Scalings DefaultScalingFor(IQuantity output)
      {
         return output.IsFraction() ? Scalings.Linear : Scalings.Log;
      }

      public bool ObservedDataMatchesOutput(DataRepository observedData, string outputPath)
      {
         var organ = observedData.ExtendedPropertyValueFor(Constants.ObservedData.ORGAN);
         var compartment = observedData.ExtendedPropertyValueFor(Constants.ObservedData.COMPARTMENT);
         var molecule = observedData.ExtendedPropertyValueFor(Constants.ObservedData.MOLECULE);

         if (organ == null || compartment == null || molecule == null)
            return false;

         return outputPath.Contains(organ) && outputPath.Contains(compartment) && outputPath.Contains(molecule);
      }
   }
}