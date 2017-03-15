using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public class TransportDiffBuilder : DiffBuilder<ITransport>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;
      private readonly IObjectComparer _objectComparer;
      private readonly IEntityPathResolver _entityPathResolver;

      public TransportDiffBuilder(ContainerDiffBuilder containerDiffBuilder, IObjectComparer objectComparer, IEntityPathResolver entityPathResolver)
      {
         _containerDiffBuilder = containerDiffBuilder;
         _objectComparer = objectComparer;
         _entityPathResolver = entityPathResolver;
      }

      public override void Compare(IComparison<ITransport> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x => _entityPathResolver.PathFor(x.TargetAmount), Captions.Diff.TargetAmount, comparison);
         CompareValues(x => _entityPathResolver.PathFor(x.SourceAmount), Captions.Diff.SourceAmount, comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}