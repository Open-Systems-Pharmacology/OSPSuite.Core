using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_ContainerTask : ContextSpecification<IContainerTask>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         sut = new ContainerTask(_objectBaseFactory, _entityPathResolver, "{0}_");
      }
   }

   internal class When_retrieving_a_sub_container_for_a_molecule_that_was_already_created : concern_for_ContainerTask
   {
      private IContainer _parentContaiener;
      private IContainer _moleculeContainer;
      private string _moleculeName;
      private IContainer _result;

      protected override void Context()
      {
         base.Context();
         _moleculeName = "a molcule";
         _parentContaiener = new Container().WithName("parentContainer");
         _moleculeContainer = new Container().WithName(_moleculeName);
         _parentContaiener.Add(_moleculeContainer);
      }

      protected override void Because()
      {
         _result = sut.CreateOrRetrieveSubContainerByName(_parentContaiener, _moleculeName);
      }

      [Observation]
      public void should_return_the_existing_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
      }
   }

   internal class When_retrieving_a_sub_container_for_a_molecule_that_was_not_already_created : concern_for_ContainerTask
   {
      private IContainer _parentContaiener;
      private IContainer _moleculeContainer;
      private string _moleculeName;
      private IContainer _result;

      protected override void Context()
      {
         base.Context();
         _moleculeName = "a molcule";
         _parentContaiener = new Container().WithName("parentContainer");
         _moleculeContainer = new Container();
         _parentContaiener.Add(_moleculeContainer);
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).Returns(_moleculeContainer);
      }

      protected override void Because()
      {
         _result = sut.CreateOrRetrieveSubContainerByName(_parentContaiener, _moleculeName);
      }

      [Observation]
      public void should_create_a_container_into_the_parent_container_and_return_the_newly_created_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
         _result.ParentContainer.ShouldBeEqualTo(_parentContaiener);
         _result.Name.ShouldBeEqualTo(_moleculeName);
      }
   }

   internal class When_removing_a_container_in_SpatialStructure : concern_for_ContainerTask
   {
      private ISpatialStructure _spatialStructure;
      private INeighborhoodBuilder _firstNeighborRemove;
      private INeighborhoodBuilder _secondNeighborRemove;
      private IContainer _containerToRemove;
      private IContainer _parent;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = A.Fake<ISpatialStructure>();
         _parent = A.Fake<IContainer>();
         _containerToRemove = A.Fake<IContainer>();
         _containerToRemove.ParentContainer = _parent;
         _firstNeighborRemove = new NeighborhoodBuilder().WithFirstNeighbor(_containerToRemove).WithSecondNeighbor(A.Fake<IContainer>());
         _secondNeighborRemove = new NeighborhoodBuilder().WithFirstNeighbor(A.Fake<IContainer>()).WithSecondNeighbor(_containerToRemove);
         A.CallTo(() => _spatialStructure.Neighborhoods).Returns(new[]
            {
               new NeighborhoodBuilder().WithFirstNeighbor(A.Fake<IContainer>()).WithSecondNeighbor(A.Fake<IContainer>()),
               _firstNeighborRemove,
               _secondNeighborRemove
            });
      }

      protected override void Because()
      {
         sut.RemoveContainerFrom(_spatialStructure, _containerToRemove);
      }

      [Observation]
      public void should_call_remove_for_firstNeighborRemove()
      {
         A.CallTo(() => _spatialStructure.RemoveNeighborhood(_firstNeighborRemove)).MustHaveHappened();
      }

      [Observation]
      public void should_call_remove_for_secondNeighborRemove()
      {
         A.CallTo(() => _spatialStructure.RemoveNeighborhood(_secondNeighborRemove)).MustHaveHappened();
      }

      [Observation]
      public void should_have_calles_remove_child_at_parent()
      {
         A.CallTo(() => _parent.RemoveChild(_containerToRemove));
      }
   }

   internal class When_asked_to_retrieve_a_unique_name_inside_a_container : concern_for_ContainerTask
   {
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("ParentContainer");
         _container.Add(new Parameter().WithName("para"));
         _container.Add(new Parameter().WithName("toto"));
         _container.Add(new Parameter().WithName("toto_1"));
         _container.Add(new Parameter().WithName("toto_3"));
         _container.Add(new Parameter().WithName("toto_tralala"));
      }

      [Observation]
      public void should_return_the_given_base_if_the_name_does_not_exist_in_the_container()
      {
         sut.CreateUniqueName(_container, "tutu", true).ShouldBeEqualTo("tutu");
      }

      [Observation]
      public void should_not_return_the_given_base_if_the_name_does_not_exist_in_the_container_and_the_used_base_name_is_set_to_false()
      {
         sut.CreateUniqueName(_container, "tutu", false).ShouldNotBeEqualTo("tutu");
      }

      [Observation]
      public void should_return_a_new_name_based_on_the_given_string_if_the_name_exists()
      {
         sut.CreateUniqueName(_container, "para").ShouldBeEqualTo("para_1");
         sut.CreateUniqueName(_container, "toto").ShouldBeEqualTo("toto_4");
      }
   }

   internal class When_retrieving_the_cache_of_children_fullfiling_a_given_criteria : concern_for_ContainerTask
   {
      private IContainer _container;
      private ICache<string, IParameter> _cache;
      private IParameter _para1;
      private IParameter _para2;
      private string _key1;
      private string _key2;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("ParentContainer");
         _container.Add(new Parameter().WithName("para"));
         _para1 = new Parameter().WithName("toto");
         _container.Add(_para1);
         _para2 = new Parameter().WithName("toto_1");
         _container.Add(_para2);
         _key1 = "toto";
         _key2 = "toto2";
         A.CallTo(() => _entityPathResolver.PathFor(_para1)).Returns(_key1);
         A.CallTo(() => _entityPathResolver.PathFor(_para2)).Returns(_key2);
      }

      protected override void Because()
      {
         _cache = sut.CacheAllChildrenSatisfying<IParameter>(_container, x => x.Name.StartsWith("toto"));
      }

      [Observation]
      public void should_create_a_cache_containg_all_the_matching_children()
      {
         _cache.ShouldOnlyContain(_para1, _para2);
      }

      [Observation]
      public void should_have_added_the_children_with_their_entity_path_into_the_cache()
      {
         _cache[_key1].ShouldBeEqualTo(_para1);
         _cache[_key2].ShouldBeEqualTo(_para2);
      }

      [Observation]
      public void should_create_a_cache_that_returns_null_if_a_value_by_key_was_not_found()
      {
         _cache["tralalal"].ShouldBeNull();
      }
   }
}