using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class CalculationMethodCacheDiffBuilder : DiffBuilder<CalculationMethodCache>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public CalculationMethodCacheDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<CalculationMethodCache> comparison)
      {
         _enumerableComparer.CompareEnumerables(comparison, x => x, item => item.Category, missingItemType: Captions.CalculationMethod);
      }
   }
}