using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.R.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Mapper
{
   public interface ISimulationToSimulationTransferMapper : IMapper<Simulation, SimulationTransfer>
   {
   }

   public class SimulationToSimulationTransferMapper : ISimulationToSimulationTransferMapper
   {
      private readonly IQuantityToOriginalQuantityValueMapper _originalQuantityValueMapper;

      public SimulationToSimulationTransferMapper(IQuantityToOriginalQuantityValueMapper originalQuantityValueMapper)
      {
         _originalQuantityValueMapper = originalQuantityValueMapper;
      }

      public SimulationTransfer MapFrom(Simulation simulation)
      {
         var simulationTransfer = new SimulationTransfer { Simulation = simulation.CoreSimulation };
         var changes = simulation.Changes;
         if (changes == null)
            return simulationTransfer;

         simulationTransfer.HasUntraceableChanges = changes.HasUntraceableChanges;
         simulationTransfer.OriginalQuantityValues = allOriginalQuantityValuesFrom(changes).ToList();
         return simulationTransfer;
      }

      /// <summary>
      ///    A change tracked before the simulation was loaded keeps its original value when the quantity is changed again.
      /// </summary>
      private IEnumerable<OriginalQuantityValue> allOriginalQuantityValuesFrom(SimulationChanges changes)
      {
         var originalQuantityValues = new Cache<string, OriginalQuantityValue>(x => x.Id);
         changes.OriginalQuantityValuesBeforeLoad.Each(originalQuantityValues.Add);

         var changesInR = changes.ChangedQuantities().Select(x => originalQuantityValueFor(x.quantity, x.initialValue))
            .Concat(changes.ChangedScaleDivisors().Select(x => originalScaleDivisorFor(x.moleculeAmount, x.initialScaleDivisor)));

         changesInR.Where(x => !originalQuantityValues.Contains(x.Id)).Each(originalQuantityValues.Add);
         return originalQuantityValues;
      }

      private OriginalQuantityValue originalQuantityValueFor(IQuantity quantity, double initialValue)
      {
         var originalQuantityValue = _originalQuantityValueMapper.MapFrom(quantity);
         originalQuantityValue.Value = initialValue;
         return originalQuantityValue;
      }

      private OriginalQuantityValue originalScaleDivisorFor(MoleculeAmount moleculeAmount, double initialScaleDivisor)
      {
         var originalQuantityValue = _originalQuantityValueMapper.MapFrom(moleculeAmount);
         originalQuantityValue.Value = initialScaleDivisor;
         return originalQuantityValue;
      }
   }
}
