using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ContainerDiffBuilder : DiffBuilder<IContainer>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public ContainerDiffBuilder(EntityDiffBuilder entityDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<IContainer> comparison)
      {
         _entityDiffBuilder.Compare(comparison);

         //compare container specific properties
         CompareValues(x => x.ContainerType, x => x.ContainerType, comparison);
         CompareValues(x => x.Mode, x => x.Mode, comparison);

         _enumerableComparer.CompareEnumerables(comparison, x => x.Children, item => item.Name);
      }
   }
}