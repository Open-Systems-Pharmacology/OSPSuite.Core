using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class EventBuilderDiffBuilder : DiffBuilder<EventBuilder>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;

      public EventBuilderDiffBuilder(EntityDiffBuilder entityDiffBuilder, EnumerableComparer enumerableComparer, IObjectComparer objectComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<EventBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.OneTime, x => x.OneTime, comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
         _enumerableComparer.CompareEnumerables(comparison, x => x.Assignments, item => item.ObjectPath);
         _enumerableComparer.CompareEnumerables(comparison, x => x.GetChildren<IEntity>(i => !i.IsAnImplementationOf<EventAssignmentBuilder>()), item => item.Name);
      }
   }
}