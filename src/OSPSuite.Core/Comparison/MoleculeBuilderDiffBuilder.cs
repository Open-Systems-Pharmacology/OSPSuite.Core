using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   internal class MoleculeBuilderDiffBuilder : DiffBuilder<IMoleculeBuilder>
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

      public override void Compare(IComparison<IMoleculeBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x=>!x.IsFloating,Captions.Diff.Stationary,comparison);
         _enumerableComparerComparer.CompareEnumerables(comparison,x=>x.UsedCalculationMethods,x=>x.Category, missingItemType: ObjectTypes.CalculationMethod);
         _objectComparer.Compare(comparison.ChildComparison(x => x.DefaultStartFormula));
         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
      }
   }

  internal class UsedCalculationMethodDiffBuilder:DiffBuilder<UsedCalculationMethod>
   {
      public override void Compare(IComparison<UsedCalculationMethod> comparison)
      {
         CompareValues(x => x.CalculationMethod, x => x.CalculationMethod, comparison);
      }
   }
}