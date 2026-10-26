using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   internal class MoleculeBuilderDiffBuilder : DiffBuilder<MoleculeBuilder>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly IObjectComparer _objectComparer;
      private readonly EnumerableComparer _enumerableComparerComparer;

      public MoleculeBuilderDiffBuilder(ContainerDiffBuilder containerDiffBuilder, IObjectComparer objectComparer, EnumerableComparer enumerableComparer)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _objectComparer = objectComparer;
         _enumerableComparerComparer = enumerableComparer;
      }

      public override void Compare(IComparison<MoleculeBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x=>!x.IsFloating,Captions.Diff.Stationary,comparison);
         _enumerableComparerComparer.CompareEnumerables(comparison,x=>x.UsedCalculationMethods,x=>x.Category, missingItemType: ObjectTypes.CalculationMethod);
         compareDefaultStartFormula(comparison);
         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
      }

      /// <summary>
      ///    A constant default start formula is the start amount of the molecule and not a hidden entity: it is compared by
      ///    value whatever the settings, as the constant formula comparison itself is skipped unless hidden entities are compared.
      /// </summary>
      private void compareDefaultStartFormula(IComparison<MoleculeBuilder> comparison)
      {
         if (bothDefaultStartFormulasAreConstant(comparison))
            CompareDoubleValues(x => constantDefaultStartValueOf(x), Captions.Diff.DefaultStartValue, comparison, x => x.DisplayUnit);
         else
            _objectComparer.Compare(comparison.ChildComparison(x => x.DefaultStartFormula));
      }

      private static bool bothDefaultStartFormulasAreConstant(IComparison<MoleculeBuilder> comparison) =>
         comparison.Object1.DefaultStartFormula is ConstantFormula && comparison.Object2.DefaultStartFormula is ConstantFormula;

      private static double constantDefaultStartValueOf(MoleculeBuilder moleculeBuilder) => ((ConstantFormula)moleculeBuilder.DefaultStartFormula).Value;
   }

  internal class UsedCalculationMethodDiffBuilder:DiffBuilder<UsedCalculationMethod>
   {
      public override void Compare(IComparison<UsedCalculationMethod> comparison)
      {
         CompareValues(x => x.CalculationMethod, x => x.CalculationMethod, comparison);
      }
   }
}