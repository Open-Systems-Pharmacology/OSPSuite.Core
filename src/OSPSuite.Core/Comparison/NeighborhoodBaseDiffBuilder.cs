using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public class NeighborhoodBaseDiffBuilder : DiffBuilder<INeighborhoodBase>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly IEntityPathResolver _entityPathResolver;

      public NeighborhoodBaseDiffBuilder(ContainerDiffBuilder containerDiffBuilder, IEntityPathResolver entityPathResolver)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _entityPathResolver = entityPathResolver;
      }

      public override void Compare(IComparison<INeighborhoodBase> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         compareNeighbors(comparison);
      }

      private void compareNeighbors(IComparison<INeighborhoodBase> comparison)
      {
         var firstNeigbor1Path = _entityPathResolver.PathFor(comparison.Object1.FirstNeighbor);
         var secondNeigbor1Path = _entityPathResolver.PathFor(comparison.Object1.SecondNeighbor);
         var firstNeigbor2Path = _entityPathResolver.PathFor(comparison.Object2.FirstNeighbor);
         var secondNeigbor2Path = _entityPathResolver.PathFor(comparison.Object2.SecondNeighbor);

         if (firstNeigbor1Path.Equals(firstNeigbor2Path) && secondNeigbor1Path.Equals(secondNeigbor2Path) ||
             firstNeigbor1Path.Equals(secondNeigbor2Path) && secondNeigbor1Path.Equals(firstNeigbor2Path))
            return;

         comparison.Add(new PropertyValueDiffItem
         {
            CommonAncestor = comparison.Object1,
            FormattedValue1 = Captions.Diff.ConnectionBetween(firstNeigbor1Path, secondNeigbor1Path),
            FormattedValue2 = Captions.Diff.ConnectionBetween(firstNeigbor2Path, secondNeigbor2Path),
            PropertyName = Captions.Diff.Connection,
            Description = Captions.Diff.PropertyDiffers(Captions.Diff.Connection, Captions.Diff.ConnectionBetween(firstNeigbor1Path, secondNeigbor1Path), Captions.Diff.ConnectionBetween(firstNeigbor2Path, secondNeigbor2Path))
         });
      }
   }
}