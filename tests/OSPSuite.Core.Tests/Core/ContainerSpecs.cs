using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_Container : ContextSpecification<IContainer>
   {
      protected IEntity _paulParameter, _ralfReaction, _stefanSpecies;
      protected IEntity _stefanSwitch;

      protected override void Context()
      {
         base.Context();
         sut = new Container();
         _paulParameter = A.Fake<IEntity>().WithName("Paul").WithId("P");
         _ralfReaction = A.Fake<IEntity>().WithName("Ralf").WithId("R");
         _stefanSpecies = A.Fake<IEntity>().WithName("Stefan").WithId("S");
         _stefanSwitch = A.Fake<IEntity>().WithName("Stefan").WithId("Sw");
      }
   }

   public class When_adding_a_child_to_a_conatainer : concern_for_Container
   {
      protected override void Because()
      {
         sut.Add(_stefanSwitch);
      }

      [Observation]
      public void should_have_added_the_entity_to_the_Children_collection()
      {
         sut.Children.ShouldOnlyContain(_stefanSwitch);
      }

      [Observation]
      public void should_have_set_the_entitys_parent_container_property_to_sut()
      {
         _stefanSwitch.ParentContainer.ShouldBeEqualTo(sut);
      }
   }

   public class When_getting_a_single_existing_child : concern_for_Container
   {
      private IEntity _result;

      protected override void Context()
      {
         base.Context();
         sut.Add(_stefanSwitch);
      }

      protected override void Because()
      {
         _result = sut.GetSingleChild<IEntity>(x => x.Name == _stefanSwitch.Name);
      }

      [Observation]
      public void should_return_the_child()
      {
         _result.ShouldBeEqualTo(_stefanSwitch);
      }
   }

   public class When_getting_existing_children : concern_for_Container
   {
      private IEnumerable<IEntity> _result;

      protected override void Context()
      {
         base.Context();
         sut.Add(_stefanSwitch);
         sut.Add(_ralfReaction);
      }

      protected override void Because()
      {
         _result = sut.GetChildren<IEntity>(x => true);
      }

      [Observation]
      public void should_return_the_child()
      {
         _result.ShouldOnlyContain(_stefanSwitch, _ralfReaction);
      }
   }

   public class When_trying_to_get_a_non_existing_child : concern_for_Container
   {
      private IEntity _result;

      protected override void Because()
      {
         _result = sut.GetSingleChild<IEntity>(x => x.Name == _stefanSwitch.Name);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_adding_a_entity_with_a_allready_existing_name : concern_for_Container
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(_stefanSpecies);
      }

      [Observation]
      public void should_throw_an_not_unique_name_exeception()
      {
         The.Action(() => sut.Add(_stefanSwitch)).ShouldThrowAn<NotUniqueNameException>();
      }
   }

   public class When_adding_a_the_same_entity_two_times : concern_for_Container
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(_stefanSpecies);
      }

      protected override void Because()
      {
         sut.Add(_stefanSpecies);
      }

      [Observation]
      public void should_change_nothing()
      {
         sut.Children.ShouldOnlyContain(_stefanSpecies);
      }
   }

   public class When_removing_a_container : concern_for_Container
   {
      private IContainer _container;
      private IContainer _childContainer;
      private IParameter _childParameter;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("C").WithId("C");

         _childContainer = new Container().WithName("c").WithId("c");
         _childParameter = new Parameter().WithName("p").WithId("p");

         _container.Add(_childContainer);
         _container.Add(_childParameter);

         sut.Add(_container);
      }

      protected override void Because()
      {
         sut.RemoveChild(_container);
      }

      [Observation]
      public void should_still_contain_its_children()
      {
         _container.GetChildren<IContainer>().ShouldOnlyContain(_childContainer);
         _container.GetChildren<IParameter>().ShouldOnlyContain(_childParameter);
      }
   }

   public class When_adding_a_null_entity_as_child_to_a_container : concern_for_Container
   {
      protected override void Because()
      {
         sut.Add(null);
      }

      [Observation]
      public void should_not_do_anythin()
      {
         sut.Children.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_retreving_a_container_and_self_with_a_predicate : concern_for_Container
   {
      private IReadOnlyList<IContainer> _results;

      protected override void Context()
      {
         base.Context();
         sut.ContainerType = ContainerType.Molecule;
         sut.Add(new Container().WithName("TOTO").WithContainerType(ContainerType.Formulation));
         var molecule = new Container().WithName("TATA").WithContainerType(ContainerType.Molecule);
         sut.Add(molecule);
         molecule.Add(new Container().WithName("TITI").WithContainerType(ContainerType.Molecule));
      }

      protected override void Because()
      {
         _results = sut.GetAllContainersAndSelf<IContainer>(x => x.ContainerType == ContainerType.Molecule);
      }

      [Observation]
      public void should_resolve_the_container_with_the_accurate_predicate_and_return_the_container_as_well()
      {
         _results.Count.ShouldBeEqualTo(3);
         _results.Any(x=>x.IsNamed("TATA")).ShouldBeTrue();
         _results.Any(x => x.IsNamed("TITI")).ShouldBeTrue();
         _results.Any(x => x.IsNamed("TOTO")).ShouldBeFalse();
         _results.Contains(sut).ShouldBeTrue();
      }
   }

   public class When_retreving_a_container_and_self_with_a_predicate_and_the_container_does_not_match_the_predicate : concern_for_Container
   {
      private IReadOnlyList<IContainer> _results;

      protected override void Context()
      {
         base.Context();
         sut.ContainerType = ContainerType.Model;
         sut.Add(new Container().WithName("TOTO").WithContainerType(ContainerType.Formulation));
         var molecule = new Container().WithName("TATA").WithContainerType(ContainerType.Molecule);
         sut.Add(molecule);
         molecule.Add(new Container().WithName("TITI").WithContainerType(ContainerType.Molecule));
      }

      protected override void Because()
      {
         _results = sut.GetAllContainersAndSelf<IContainer>(x => x.ContainerType == ContainerType.Molecule);
      }

      [Observation]
      public void should_resolve_the_container_with_the_accurate_predicate_and_not_return_the_container()
      {
         _results.Count.ShouldBeEqualTo(2);
         _results.Any(x => x.IsNamed("TATA")).ShouldBeTrue();
         _results.Any(x => x.IsNamed("TITI")).ShouldBeTrue();
         _results.Any(x => x.IsNamed("TOTO")).ShouldBeFalse();
         _results.Contains(sut).ShouldBeFalse();
      }
   }
}