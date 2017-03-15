using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_EntityPathResolver : ContextSpecification<IEntityPathResolver>
   {
      protected IParameter _parameter;

      protected override void Context()
      {
         sut = new EntityPathResolver(new ObjectPathFactory(new AliasCreator()));
         _parameter = new Parameter().WithName("P1");
         _parameter.ParentContainer = new Container().WithName("C1");
      }
   }

   public class when_getting_the_path_of_an_enity : concern_for_EntityPathResolver
   {
      protected string _path;

      protected override void Context()
      {
         base.Context();
         _parameter.RootContainer.ParentContainer = new Container().WithName("TRalala");
      }

      protected override void Because()
      {
         _path = sut.PathFor(_parameter);
      }

      [Observation]
      public void should_have_not_added_the_root_container_at_the_beginning()
      {
         _path.StartsWith(Constants.ROOT).ShouldBeFalse();
      }
   }

   public class When_the_path_of_an_entity_starts_with_the_element_root : concern_for_EntityPathResolver
   {
      protected string _path;

      protected override void Context()
      {
         base.Context();
         _parameter.RootContainer.ParentContainer = new Container().WithName(Constants.ROOT);
      }

      protected override void Because()
      {
         _path = sut.PathFor(_parameter);
      }

      [Observation]
      public void should_have_removed_the_root_entry()
      {
         _path.StartsWith(Constants.ROOT).ShouldBeFalse();
      }

      [Observation]
      public void should_end_with_entity_name()
      {
         _path.EndsWith(ObjectPath.PATH_DELIMITER + _parameter.Name).ShouldBeTrue();
      }
   }

   public class When_resolving_the_path_for_an_object_defined_in_a_top_container : concern_for_EntityPathResolver
   {
      private IContainer _topContainer;
      private Container _sim;

      protected override void Context()
      {
         base.Context();
         _sim = new Container().WithName("Sim").WithContainerType(ContainerType.Simulation);
         _topContainer = new Container()
            .WithParentContainer(_sim);
         _parameter.ParentContainer.WithParentContainer(_topContainer);
      }

   
      [Observation]
      public void should_remove_the_name_of_the_simulation_when_the_top_container_is_an_organism()
      {
         _topContainer.Name = Constants.ORGANISM;
         sut.PathFor(_parameter).Contains("Sim").ShouldBeFalse();
      }

      [Observation]
      public void should_remove_the_name_of_the_simulation_when_the_top_container_is_an_application_sets()
      {
         _topContainer.Name = Constants.APPLICATIONS;
         sut.PathFor(_parameter).Contains("Sim").ShouldBeFalse();
      }

      [Observation]
      public void should_remove_the_name_of_the_simulation_when_the_top_container_is_the_neighborhoods_container()
      {
         _topContainer.Name = Constants.NEIGHBORHOODS;
         sut.PathFor(_parameter).Contains("Sim").ShouldBeFalse();
      }
   }

   public class When_creating_a_path_for_an_entity_in_a_simulation : concern_for_EntityPathResolver
   {
      private IContainer _topContainer;
      private Container _sim;

      protected override void Context()
      {
         base.Context();
         _sim = new Container().WithName("Sim");
         _sim.ContainerType = ContainerType.Simulation;
         _topContainer = new Container().WithName("TOTO")
            .WithParentContainer(_sim);
         _parameter.ParentContainer.WithParentContainer(_topContainer);
      }

      [Observation]
      public void should_have_removed_the_first_entry()
      {
         sut.ObjectPathFor(_parameter)[0].ShouldBeEqualTo("TOTO");
      }
   }

   public class When_creating_a_path_for_an_entity_that_is_not_defined_anywhere : concern_for_EntityPathResolver
   {
      [Observation]
      public void should_have_removed_the_first_entry()
      {
         sut.ObjectPathFor(_parameter.ParentContainer)[0].ShouldBeEqualTo("C1");
      }
   }
}