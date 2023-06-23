using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class EventAssignmentBuilderDiffBuilder : DiffBuilder<EventAssignmentBuilder>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly IObjectComparer _objectComparer;

      public EventAssignmentBuilderDiffBuilder(EntityDiffBuilder entityDiffBuilder, IObjectComparer objectComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<EventAssignmentBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.UseAsValue, x => x.UseAsValue, comparison);
         _objectComparer.Compare(comparison.DimensionComparison());
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }
}