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

      /// <summary>
      ///    A quantity calculated initially and still calculated has not been changed, even if its value changed with the
      ///    quantities it depends on. One that was overwritten since was initially worth the value of its formula.
      ///    The percentile of a distributed parameter follows the value of the parameter, so it is not reported when the
      ///    distributed parameter itself is.
      /// </summary>
      public IEnumerable<(IQuantity quantity, double initialValue)> ChangedQuantities()
      {
         var changedQuantities = _initialValues.Where(x => !(x.Value.IsCalculated && isCalculated(x.Key)))
            .Select(x => (quantity: x.Key, initialValue: x.Value.IsCalculated ? formulaValueOf(x.Key) : x.Value.Value))
            .Where(x => !currentValueOf(x.quantity).Equals(x.initialValue))
            .ToList();

         var changedDistributedParameters = new HashSet<IQuantity>(changedQuantities.Select(x => x.quantity).OfType<IDistributedParameter>(), ReferenceEqualityComparer.Instance);
         return changedQuantities.Where(x => !isPercentileOfAny(x.quantity, changedDistributedParameters));
      }

      public IEnumerable<(MoleculeAmount moleculeAmount, double initialScaleDivisor)> ChangedScaleDivisors() =>
         _initialScaleDivisors.Where(x => !x.Key.ScaleDivisor.Equals(x.Value)).Select(x => (x.Key, x.Value));

      /// <summary>
      ///    Calculated values are not read here: a calculated value changes with the quantities it depends on, and reading
      ///    it would not terminate for a formula with a circular reference.
      /// </summary>
      private void storeInitialValueOf(IQuantity quantity)
      {
         _initialValues[quantity] = isCalculated(quantity) ? InitialValue.Calculated : new InitialValue(quantity.Value, IsCalculated: false);

         if (quantity is MoleculeAmount moleculeAmount)
            _initialScaleDivisors[moleculeAmount] = moleculeAmount.ScaleDivisor;
      }

      private double currentValueOf(IQuantity quantity)
      {
         if (isCalculated(quantity) && hasCircularReference(quantity))
            return double.NaN;

         return quantity.Value;
      }

      private double formulaValueOf(IQuantity quantity)
      {
         if (hasCircularReference(quantity))
            return double.NaN;

         if (quantity is IDistributedParameter distributedParameter && _initialValues.TryGetValue(distributedParameter.PercentileParameter, out var initialPercentile) && !initialPercentile.IsCalculated)
            return distributedParameter.ValueFor(initialPercentile.Value);

         return quantity.Formula.TryCalculate(quantity).value;
      }

      private static bool isPercentileOfAny(IQuantity quantity, HashSet<IQuantity> distributedParameters) =>
         quantity.IsNamed(Constants.Distribution.PERCENTILE) && quantity.ParentContainer is IDistributedParameter parent && distributedParameters.Contains(parent);

      private static bool isCalculated(IQuantity quantity) => !quantity.IsFixedValue && !quantity.Formula.IsConstant();

      private static bool hasCircularReference(IUsingFormula usingFormula) =>
         hasCircularReference(usingFormula, new HashSet<IUsingFormula>(ReferenceEqualityComparer.Instance), new HashSet<IUsingFormula>(ReferenceEqualityComparer.Instance));

      /// <summary>
      ///    Follows the references a formula reads when it is calculated. A reference to a fixed value ends the path since
      ///    its formula is not calculated, except for the quantity the check started from.
      /// </summary>
      private static bool hasCircularReference(IUsingFormula usingFormula, HashSet<IUsingFormula> path, HashSet<IUsingFormula> checkedObjects)
      {
         if (checkedObjects.Contains(usingFormula))
            return false;

         if (!path.Add(usingFormula))
            return true;

         var circular = referencedObjectsOf(usingFormula)
            .Where(x => path.Contains(x) || usesFormulaForValue(x))
            .Any(x => hasCircularReference(x, path, checkedObjects));

         path.Remove(usingFormula);
         checkedObjects.Add(usingFormula);
         return circular;
      }

      private static IEnumerable<IUsingFormula> referencedObjectsOf(IUsingFormula usingFormula)
      {
         var formula = usingFormula.Formula;
         if (formula == null)
            return Enumerable.Empty<IUsingFormula>();

         if (formula.AreReferencesResolved)
            return formula.ObjectReferences.Select(x => x.Object).OfType<IUsingFormula>();

         return formula.ObjectPaths.Select(x => x.TryResolve<IUsingFormula>(usingFormula)).Where(x => x != null);
      }

      private static bool usesFormulaForValue(IUsingFormula usingFormula) => !(usingFormula is IQuantity quantity) || !quantity.IsFixedValue;

      private readonly record struct InitialValue(double Value, bool IsCalculated)
      {
         public static InitialValue Calculated => new InitialValue(double.NaN, IsCalculated: true);
      }
   }
}
