using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EntitySources : ContextSpecification<EntitySources>
   {
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         sut = new EntitySources();
         _buildingBlock = new ObserverBuildingBlock().WithName("BBName").WithId("BBId");
      }
   }

   public class When_adding_an_entity_source_without_an_entity_path : concern_for_EntitySources
   {
      protected override void Because()
      {
         sut.Add(new EntitySource(_buildingBlock, "sourcePath", null));
      }

      [Observation]
      public void should_ignore_the_entry()
      {
         sut.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_cloning_an_entity_sources : concern_for_EntitySources
   {
      private EntitySources _clone;

      protected override void Context()
      {
         base.Context();
         sut.Add(new EntitySource(_buildingBlock, "sourcePath1", new Parameter()) {EntityPath = "A"});
         sut.Add(new EntitySource(_buildingBlock, "sourcePath2", new Parameter()) {EntityPath = "B"});
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_return_a_clone_of_the_sources_without_the_reference_to_the_original_object()
      {
         _clone.Count.ShouldBeEqualTo(2);
         _clone.ElementAt(0).Source.ShouldBeNull();
         _clone.ElementAt(1).Source.ShouldBeNull();
      }
   }
}