using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

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
         //both drive model construction: global molecule parameters are only created for floating xenobiotic molecules and
         //the start formula is mapped differently for amount and concentration based molecules
         CompareValues(x => x.IsXenobiotic, x => x.IsXenobiotic, comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         //copied to every molecule amount of the model and used to classify outputs and PK analyses
         CompareValues(x => x.QuantityType, x => x.QuantityType, comparison);
         _enumerableComparerComparer.CompareEnumerables(comparison,x=>x.UsedCalculationMethods,x=>x.Category, missingItemType: ObjectTypes.CalculationMethod);
         _objectComparer.Compare(comparison.ChildComparison(x => x.DefaultStartFormula));
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