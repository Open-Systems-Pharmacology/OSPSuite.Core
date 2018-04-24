using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ReactionDiffBuilder : DiffBuilder<IReaction>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;

      public ReactionDiffBuilder(IObjectComparer objectComparer, ContainerDiffBuilder containerDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IReaction> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
         _enumerableComparer.CompareEnumerables(comparison, x => x.Educts, item => item.Partner.Name);
         _enumerableComparer.CompareEnumerables(comparison, x => x.Products, item => item.Partner.Name);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ModifierNames, item => item, missingItemType: ObjectTypes.Modifier);
      }
   }
}