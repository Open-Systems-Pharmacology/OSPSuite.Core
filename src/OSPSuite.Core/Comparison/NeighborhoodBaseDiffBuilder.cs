using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public abstract class NeighborhoodBaseDiffBuilder<TNeighborhood> : DiffBuilder<TNeighborhood> where TNeighborhood : class, IContainer
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;

      protected NeighborhoodBaseDiffBuilder(ContainerDiffBuilder containerDiffBuilder)
      {
         _containerDiffBuilder = containerDiffBuilder;
      }

      public override void Compare(IComparison<TNeighborhood> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         compareNeighbors(comparison);
      }

      private void compareNeighbors(IComparison<TNeighborhood> comparison)
      {
         var (firstNeighbor1Path, secondNeighbor1Path, firstNeighbor2Path, secondNeighbor2Path) = GetNeighborsComparisonPath(comparison);

         if (firstNeighbor1Path.Equals(firstNeighbor2Path) && secondNeighbor1Path.Equals(secondNeighbor2Path) ||
             firstNeighbor1Path.Equals(secondNeighbor2Path) && secondNeighbor1Path.Equals(firstNeighbor2Path))
            return;

         comparison.Add(new PropertyValueDiffItem
         {
            CommonAncestor = comparison.Object1,
            FormattedValue1 = Captions.Diff.ConnectionBetween(firstNeighbor1Path, secondNeighbor1Path),
            FormattedValue2 = Captions.Diff.ConnectionBetween(firstNeighbor2Path, secondNeighbor2Path),
            PropertyName = Captions.Diff.Connection,
            Description = Captions.Diff.PropertyDiffers(Captions.Diff.Connection, Captions.Diff.ConnectionBetween(firstNeighbor1Path, secondNeighbor1Path), Captions.Diff.ConnectionBetween(firstNeighbor2Path, secondNeighbor2Path))
         });
      }

      protected abstract (ObjectPath firstNeighbor1Path, ObjectPath secondNeighbor1Path, ObjectPath firstNeighbor2Path, ObjectPath secondNeighbor2Path) GetNeighborsComparisonPath(IComparison<TNeighborhood> comparison);
   }

   public class NeighborhoodBuilderDiffBuilder : NeighborhoodBaseDiffBuilder<NeighborhoodBuilder>
   {
      public NeighborhoodBuilderDiffBuilder(ContainerDiffBuilder containerDiffBuilder) : base(containerDiffBuilder)
      {
      }

      protected override (ObjectPath firstNeighbor1Path, ObjectPath secondNeighbor1Path, ObjectPath firstNeighbor2Path, ObjectPath secondNeighbor2Path) GetNeighborsComparisonPath(IComparison<NeighborhoodBuilder> comparison)
      {
         return (comparison.Object1.FirstNeighborPath, comparison.Object1.SecondNeighborPath, comparison.Object2.FirstNeighborPath, comparison.Object2.SecondNeighborPath);
      }
   }

   public class NeighborhoodDiffBuilder : NeighborhoodBaseDiffBuilder<Neighborhood>
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public NeighborhoodDiffBuilder(ContainerDiffBuilder containerDiffBuilder, IEntityPathResolver entityPathResolver) : base(containerDiffBuilder)
      {
         _entityPathResolver = entityPathResolver;
      }

      protected override (ObjectPath firstNeighbor1Path, ObjectPath secondNeighbor1Path, ObjectPath firstNeighbor2Path, ObjectPath secondNeighbor2Path) GetNeighborsComparisonPath(IComparison<Neighborhood> comparison)
      {
         var firstNeighbor1Path = _entityPathResolver.ObjectPathFor(comparison.Object1.FirstNeighbor);
         var secondNeighbor1Path = _entityPathResolver.ObjectPathFor(comparison.Object1.SecondNeighbor);
         var firstNeighbor2Path = _entityPathResolver.ObjectPathFor(comparison.Object2.FirstNeighbor);
         var secondNeighbor2Path = _entityPathResolver.ObjectPathFor(comparison.Object2.SecondNeighbor);

         return (firstNeighbor1Path, secondNeighbor1Path, firstNeighbor2Path, secondNeighbor2Path);
      }
   }
}