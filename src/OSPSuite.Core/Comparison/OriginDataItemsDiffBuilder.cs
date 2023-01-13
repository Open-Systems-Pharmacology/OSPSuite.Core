using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class OriginDataItemsDiffBuilder : DiffBuilder<OriginDataItems>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public OriginDataItemsDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }
      public override void Compare(IComparison<OriginDataItems> comparison)
      {
         _enumerableComparer.CompareEnumerables(comparison, x => x.AllDataItems, x => x.Name);
      }
   }
}