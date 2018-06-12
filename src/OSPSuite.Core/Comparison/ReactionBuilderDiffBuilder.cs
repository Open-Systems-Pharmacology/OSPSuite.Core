using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class ReactionBuilderDiffBuilder : DiffBuilder<IReactionBuilder>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;

      public ReactionBuilderDiffBuilder(IObjectComparer objectComparer, ContainerDiffBuilder containerDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IReactionBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
         _enumerableComparer.CompareEnumerables(comparison, x => x.Educts, item => item.MoleculeName);
         _enumerableComparer.CompareEnumerables(comparison, x => x.Products, item => item.MoleculeName);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ModifierNames, item => item, missingItemType: ObjectTypes.Modifier);
         CompareValues(x => x.ContainerCriteria, x => x.ContainerCriteria, comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
      }
   }
}