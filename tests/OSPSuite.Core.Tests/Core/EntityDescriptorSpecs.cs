using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_EntityDescriptor : ContextSpecification<EntityDescriptor>
   {
      protected IEntity _entity;
      private Container _parent;
      private Container _grandParent;

      protected override void Context()
      {
         _entity = new Parameter().WithName("P1");
         _entity.AddTag("P1_TAG1");

         _parent = new Container().WithName("Parent");
         _parent.AddTag("Parent_TAG1");

         _grandParent = new Container().WithName("GrandParent");
         _grandParent.AddTag("GrandParent_TAG1");

         _parent.Add(_entity);
         _grandParent.Add(_parent);

         sut = new EntityDescriptor(_entity);
      }
   }


   public class When_retrieving_the_tag_of_an_entity : concern_for_EntityDescriptor
   {
      [Observation]
      public void should_return_its_direct_tags()
      {
         sut.Tags.Select(x=>x.Value).ShouldOnlyContain("P1", "P1_TAG1");
      }
   }
   
   public class When_retrieving_the_container_tag_of_an_entity : concern_for_EntityDescriptor
   {
      [Observation]
      public void should_return_all_tags_from_its_parent_container()
      {
         sut.ParentContainerTags.Select(x => x.Value).ShouldOnlyContain("Parent", "Parent_TAG1","GrandParent","GrandParent_TAG1");
      }
   }

   public class When_creating_an_entity_descriptor_for_an_entity_hierarchy : concern_for_EntityDescriptor
   {
      private int _createDescriptorCount = 0;

      protected override void Context()
      {
         base.Context();
         
         EntityDescriptor CreateDescriptor (IEntity entity)
         {
            if (entity == null)
               return null;
            _createDescriptorCount++;
            return new EntityDescriptor(entity, CreateDescriptor);
         }

         sut = new EntityDescriptor(_entity, CreateDescriptor);
      }

      [Observation]
      public void should_not_load_the_entity_descriptor_of_the_parent_containers_until_required()
      {
         _createDescriptorCount.ShouldBeEqualTo(0);
         var tags = sut.ParentContainerTags;
         _createDescriptorCount.ShouldBeEqualTo(2);
      }
   }
}	