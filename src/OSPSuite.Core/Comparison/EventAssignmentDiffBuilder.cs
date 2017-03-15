using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class EventAssignmentDiffBuilder : DiffBuilder<IEventAssignment>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly IObjectComparer _objectComparer;

      public EventAssignmentDiffBuilder(EntityDiffBuilder entityDiffBuilder, IObjectComparer objectComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IEventAssignment> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.UseAsValue, x => x.UseAsValue, comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
         // Not Nessessary to compare Changed Object. 
      }
   }
}