using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   /// <summary>
   ///    Changes made to a simulation since it was created from its building blocks. Changes made before the simulation
   ///    was loaded are kept as they were saved. Changes made afterwards are found by comparing the parameter values,
   ///    initial values and scale divisors with the values they had when this object was created, so that values set
   ///    directly on a quantity are found as well.
   /// </summary>
   public class SimulationChanges
   {
      private readonly Dictionary<IQuantity, InitialValue> _initialValues = new Dictionary<IQuantity, InitialValue>(ReferenceEqualityComparer.Instance);
      private readonly Dictionary<MoleculeAmount, double> _initialScaleDivisors = new Dictionary<MoleculeAmount, double>(ReferenceEqualityComparer.Instance);

      public IReadOnlyList<OriginalQuantityValue> OriginalQuantityValuesBeforeLoad { get; }
      public bool HasUntraceableChanges { get; }

      public SimulationChanges(IContainer root, IEnumerable<OriginalQuantityValue> originalQuantityValuesBeforeLoad, bool hasUntraceableChanges)
      {
         OriginalQuantityValuesBeforeLoad = originalQuantityValuesBeforeLoad.ToList();
         HasUntraceableChanges = hasUntraceableChanges;
         root.GetAllChildren<IQuantity>(x => x is IParameter || x is MoleculeAmount).Each(storeInitialValueOf);
      }

      public IEnumerable<(IQuantity quantity, double initialValue)> ChangedQuantities() =>
         _initialValues.Where(x => hasChanged(x.Key, x.Value)).Select(x => (x.Key, x.Value.Value));

      public IEnumerable<(MoleculeAmount moleculeAmount, double initialScaleDivisor)> ChangedScaleDivisors() =>
         _initialScaleDivisors.Where(x => !x.Key.ScaleDivisor.Equals(x.Value)).Select(x => (x.Key, x.Value));

      private void storeInitialValueOf(IQuantity quantity)
      {
         _initialValues[quantity] = new InitialValue(quantity.TryGetValue().value, isCalculated(quantity));

         if (quantity is MoleculeAmount moleculeAmount)
            _initialScaleDivisors[moleculeAmount] = moleculeAmount.ScaleDivisor;
      }

      /// <summary>
      ///    A value calculated from a formula changes with the quantities it depends on. It is only a change of its own
      ///    once the quantity no longer uses its formula.
      /// </summary>
      private static bool hasChanged(IQuantity quantity, InitialValue initialValue)
      {
         if (initialValue.IsCalculated && isCalculated(quantity))
            return false;

         return !quantity.Value.Equals(initialValue.Value);
      }

      private static bool isCalculated(IQuantity quantity) => !quantity.IsFixedValue && !quantity.Formula.IsConstant();

      private readonly record struct InitialValue(double Value, bool IsCalculated);
   }
}
