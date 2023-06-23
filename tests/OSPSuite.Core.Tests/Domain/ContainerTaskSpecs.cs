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
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         sut = new ContainerTask(_objectBaseFactory, _entityPathResolver, _objectPathFactory);
      }
   }

   internal class When_retrieving_a_sub_container_for_a_molecule_that_was_already_created : concern_for_ContainerTask
   {
      private IContainer _parentContainer;
      private IContainer _moleculeContainer;
      private string _moleculeName;
      private IContainer _result;

      protected override void Context()
      {
         base.Context();
         _moleculeName = "a molcule";
         _parentContainer = new Container().WithName("parentContainer");
         _moleculeContainer = new Container().WithName(_moleculeName);
         _parentContainer.Add(_moleculeContainer);
      }

      protected override void Because()
      {
         _result = sut.CreateOrRetrieveSubContainerByName(_parentContainer, _moleculeName);
      }

      [Observation]
      public void should_return_the_existing_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
      }
   }

   internal class When_retrieving_a_sub_container_for_a_molecule_that_was_not_already_created : concern_for_ContainerTask
   {
      private IContainer _parentContainer;
      private IContainer _moleculeContainer;
      private string _moleculeName;
      private IContainer _result;

      protected override void Context()
      {
         base.Context();
         _moleculeName = "a molcule";
         _parentContainer = new Container().WithName("parentContainer");
         _moleculeContainer = new Container();
         _parentContainer.Add(_moleculeContainer);
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).Returns(_moleculeContainer);
      }

      protected override void Because()
      {
         _result = sut.CreateOrRetrieveSubContainerByName(_parentContainer, _moleculeName);
      }

      [Observation]
      public void should_create_a_container_into_the_parent_container_and_return_the_newly_created_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
         _result.ParentContainer.ShouldBeEqualTo(_parentContainer);
         _result.Name.ShouldBeEqualTo(_moleculeName);
      }
   }

   internal class When_removing_a_container_in_SpatialStructure : concern_for_ContainerTask
   {
      private SpatialStructure _spatialStructure;
      private NeighborhoodBuilder _firstNeighborRemove;
      private NeighborhoodBuilder _secondNeighborRemove;
      private IContainer _containerToRemove;
      private IContainer _parent;
      private ObjectPath _containerToRemovePath;
      private NeighborhoodBuilder _thirdNeighborhood;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure();
         _spatialStructure.NeighborhoodsContainer = new Container();
         _parent = A.Fake<IContainer>();
         _containerToRemove = A.Fake<IContainer>();
         _containerToRemove.ParentContainer = _parent;
         _containerToRemovePath = new ObjectPath("containerToRemovePath");
         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(_containerToRemove)).Returns(_containerToRemovePath);
         _firstNeighborRemove = new NeighborhoodBuilder
         {
            FirstNeighborPath = _containerToRemovePath,
            SecondNeighborPath = new ObjectPath("anotherContainer"),
            Name = "firstNeighborRemove"
         };
         _secondNeighborRemove = new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("anotherContainer"),
            SecondNeighborPath = _containerToRemovePath,
            Name = "secondNeighborRemove"
         };
         _thirdNeighborhood = new NeighborhoodBuilder {Name = "thirdNeighborhood"};
         _spatialStructure.AddNeighborhood(_firstNeighborRemove);
         _spatialStructure.AddNeighborhood(_secondNeighborRemove);
         _spatialStructure.AddNeighborhood(_thirdNeighborhood);
      }

      protected override void Because()
      {
         sut.RemoveContainerFrom(_spatialStructure, _containerToRemove);
      }

      [Observation]
      public void should_remove_the_neighborhood_connected_to_the_container_to_remove()
      {
         _spatialStructure.Neighborhoods.ShouldOnlyContain(_thirdNeighborhood);
      }

      [Observation]
      public void should_have_called_remove_child_at_parent()
      {
         A.CallTo(() => _parent.RemoveChild(_containerToRemove)).MustHaveHappened();
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
         _container.Add(new Parameter().WithName("toto 1"));
         _container.Add(new Parameter().WithName("toto 3"));
         _container.Add(new Parameter().WithName("toto tralala"));
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
         sut.CreateUniqueName(_container, "para").ShouldBeEqualTo("para 1");
         sut.CreateUniqueName(_container, "toto").ShouldBeEqualTo("toto 4");
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
      public void should_create_a_cache_containing_all_the_matching_children()
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