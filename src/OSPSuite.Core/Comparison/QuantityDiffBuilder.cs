using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class QuantityDiffBuilder : DiffBuilder<IQuantity>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly WithValueOriginComparison<IQuantity> _withValueOriginComparison;

      public QuantityDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<IQuantity> withValueOriginComparison)
      {
         _objectComparer = objectComparer;
         _entityDiffBuilder = entityDiffBuilder;
         _withValueOriginComparison = withValueOriginComparison;
      }

      public override void Compare(IComparison<IQuantity> comparison)
      {
         _withValueOriginComparison.AddValueOriginToComparison(comparison, this, compareQuantities);
      }

      private void compareQuantities(IComparison<IQuantity> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
         CompareValues(x => x.NegativeValuesAllowed, x => x.NegativeValuesAllowed, comparison);

         if (!comparison.Settings.OnlyComputingRelevant)
         {
            CompareValues(x => x.Persistable, x => x.Persistable, comparison);
            CompareValues(x => x.DisplayUnit, Captions.DisplayUnit, comparison);
         }

         if (shouldCompareValues(comparison))
            compareValues(comparison);
         else
            _objectComparer.Compare(comparison.FormulaComparison());
      }

      private void compareValues(IComparison<IQuantity> comparison)
      {
         var numberOfDifferencesBeforeComparison = comparison.Report.Count;
         CompareDoubleValues(x => x.Value, x => x.Value, comparison, x => x.DisplayUnit);

         //values differ: the difference is already reported
         if (comparison.Report.Count != numberOfDifferencesBeforeComparison)
            return;

         compareFixedValueOverridingFormula(comparison);
      }

      /// <summary>
      ///    A fixed value is exported as a constant whereas a formula stays active in the model. Equal values therefore hide a
      ///    different behavior over time when only one quantity is fixed and the other one uses a non constant formula.
      /// </summary>
      private void compareFixedValueOverridingFormula(IComparison<IQuantity> comparison)
      {
         if (!comparison.ComparedObjectsDefined)
            return;

         var q1 = comparison.Object1;
         var q2 = comparison.Object2;
         if (q1.IsFixedValue == q2.IsFixedValue)
            return;

         var activeFormula = q1.IsFixedValue ? q2.Formula : q1.Formula;
         if (activeFormula == null || activeFormula.IsConstant())
            return;

         CompareValues(x => x.IsFixedValue, x => x.IsFixedValue, comparison);
      }

      private static bool shouldCompareValues(IComparison<IQuantity> comparison)
      {
         if (!comparison.ComparedObjectsDefined || comparison.Settings.FormulaComparison == FormulaComparison.Value)
            return true;

         //we are comparing formulas. We need to check if the value was overwritten by the user
         var q1 = comparison.Object1;
         var q2 = comparison.Object2;

         if (shouldCompareConstantFormulasAsValue(comparison, q1, q2))
            return true;

         return q1.IsFixedValue || q2.IsFixedValue;
      }

      private static bool shouldCompareConstantFormulasAsValue(IComparison<IQuantity> comparison, IQuantity q1, IQuantity q2)
      {
         return bothFormulasAreConstant(q1, q2) && !comparison.Settings.CompareHiddenEntities;
      }

      private static bool bothFormulasAreConstant(IQuantity q1, IQuantity q2)
      {
         return formulaIsConstantIn(q1) && formulaIsConstantIn(q2);
      }

      private static bool formulaIsConstantIn(IQuantity quantity)
      {
         return quantity.Formula.IsConstant();
      }
   }
}