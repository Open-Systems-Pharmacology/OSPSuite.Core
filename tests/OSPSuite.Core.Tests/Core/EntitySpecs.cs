using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_Entity : ContextSpecification<IEntity>
   {
      protected IContainer _entityParentContainer;

      protected override void Context()
      {
         sut = new Parameter().WithName("PARA");
         _entityParentContainer = new Container().WithName("SUB_CONT1");
         _entityParentContainer.Add(sut);
      }
   }

   class When_Upate_properties_is_called_for_an_Entity : concern_for_Entity
   {
      private IEntity _result;
      private ICloneManager _cloneManger;
      private Tag _tag = new Tag("Tig");

      protected override void Context()
      {
         base.Context();
         sut.AddTag(_tag);
         _cloneManger = A.Fake<ICloneManager>();
      }

      protected override void Because()
      {
         _result = new Parameter();
         _result.UpdatePropertiesFrom(sut,_cloneManger);
      }

      [Observation]
      public void should_have_update_the_properties()
      {
         _result.Name.ShouldBeEqualTo(sut.Name);
      }

      [Observation]
      public void should_clone_the_tag()
      {
         var tag = _result.Tags.FirstOrDefault();
         ReferenceEquals(tag,_tag).ShouldBeFalse();
         
         tag.Value.ShouldBeEqualTo(_tag.Value);
      }
   }


   public class When_resolving_the_root_container_of_an_entity_in_a_hierarchy_where_no_root_container_was_defined : concern_for_Entity
   {
      private IContainer _topContainer;

      protected override void Context()
      {
         base.Context();
         _topContainer = new Container().WithName("TOP_CONTAINER");
         _topContainer.Add(_entityParentContainer);
      }

      [Observation]
      public void should_return_the_top_container()
      {
         sut.RootContainer.ShouldBeEqualTo(_topContainer);
      }
   }

   
   public class When_resolving_the_root_container_of_an_entity_in_a_hierarchy_where_a_root_container_was_defined : concern_for_Entity
   {
      private IContainer _rootContainer;

      protected override void Context()
      {
         base.Context();
         _rootContainer = new ARootContainer().WithName("ROOT_CONTAINER");
         var topContainer = new Container().WithName("TOP_CONTAINER");
         _rootContainer.Add(_entityParentContainer);
         topContainer.Add(_rootContainer);
      }

      [Observation]
      public void should_return_the_root_container_instead_of_the_top_container()
      {
         sut.RootContainer.ShouldBeEqualTo(_rootContainer);
      }
   }

   
   public class When_resolving_the_root_container_of_an_entity_that_is_the_root_container : concern_for_Entity
   {
      private IContainer _rootContainer;

      protected override void Context()
      {
         _rootContainer = new ARootContainer().WithName("ROOT_CONTAINER");
      }

      [Observation]
      public void should_return_the_entity()
      {
         _rootContainer.RootContainer.ShouldBeEqualTo(_rootContainer);
      }
   }
   
   public class When_checking_if_an_entity_contains_an_ancestor_with_a_given_name : concern_for_Entity
   {
      private IContainer _rootContainer;

      protected override void Context()
      {
         base.Context();
         _rootContainer = new ARootContainer().WithName("ROOT_CONTAINER");
         var topContainer = new Container().WithName("TOP_CONTAINER");
         _rootContainer.Add(_entityParentContainer);
         topContainer.Add(_rootContainer);
      }

      [Observation]
      public void should_return_true_if_an_ancestor_exists_for_the_given_name()
      {
         sut.HasAncestorNamed(_entityParentContainer.Name).ShouldBeTrue();
         sut.HasAncestorNamed("ROOT_CONTAINER").ShouldBeTrue();
         sut.HasAncestorNamed("TOP_CONTAINER").ShouldBeTrue();
         _entityParentContainer.HasAncestorNamed("TOP_CONTAINER").ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_an_ancestor_doest_exist_for_the_given_name()
      {
         sut.HasAncestorNamed("tralala").ShouldBeFalse();
         _rootContainer.HasAncestorNamed("ROOT_CONTAINER").ShouldBeFalse();
      }
   }
} 