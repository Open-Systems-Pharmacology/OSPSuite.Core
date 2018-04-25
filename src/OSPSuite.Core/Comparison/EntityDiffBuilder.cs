using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class EntityDiffBuilder : DiffBuilder<IEntity>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComprer;

      public EntityDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComprer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComprer = enumerableComprer;
      }

      public override void Compare(IComparison<IEntity> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _enumerableComprer.CompareEnumerables(comparison, x => x.Tags, x => x.Value, missingItemType: Captions.Diff.Tag);
      }
   }
}