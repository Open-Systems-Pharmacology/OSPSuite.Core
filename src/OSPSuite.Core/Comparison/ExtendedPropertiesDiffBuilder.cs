using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ExtendedPropertiesDiffBuilder : DiffBuilder<ExtendedProperties>
   {
      private readonly EnumerableComparer _enumerableComparer;

      public ExtendedPropertiesDiffBuilder(EnumerableComparer enumerableComparer)
      {
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<ExtendedProperties> comparison)
      {
         _enumerableComparer.CompareEnumerables(comparison, x => x, x => x.Name);
      }
   }
}