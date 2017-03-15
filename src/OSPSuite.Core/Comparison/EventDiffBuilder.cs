using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public class EventDiffBuilder : DiffBuilder<IEvent>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;
      private readonly IEntityPathResolver _entityPathResolver;

      public EventDiffBuilder(EntityDiffBuilder entityDiffBuilder, EnumerableComparer enumerableComparer, IObjectComparer objectComparer, IEntityPathResolver entityPathResolver)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
         _entityPathResolver = entityPathResolver;
      }

      public override void Compare(IComparison<IEvent> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.OneTime, x => x.OneTime, comparison);
         _objectComparer.Compare(comparison.FormulaComparison());
         _enumerableComparer.CompareEnumerables(comparison, x => x.Assignments, item => _entityPathResolver.ObjectPathFor(item.ChangedEntity));
         _enumerableComparer.CompareEnumerables(comparison, x => x.GetChildren<IEntity>(i => !i.IsAnImplementationOf<IEventAssignment>()), item => item.Name);
      }
   }
}