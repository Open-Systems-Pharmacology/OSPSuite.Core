using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class EventGroupBuilderDiffBuilder : DiffBuilder<IEventGroupBuilder>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;

      public EventGroupBuilderDiffBuilder(ContainerDiffBuilder containerDiffBuilder)
      {
         _containerDiffBuilder = containerDiffBuilder;
      }

      public override void Compare(IComparison<IEventGroupBuilder> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareValues(x => x.SourceCriteria, x => x.SourceCriteria, comparison);
      }
   }
}