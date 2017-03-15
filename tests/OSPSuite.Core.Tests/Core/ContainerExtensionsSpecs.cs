using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_ContainerExtensions : StaticContextSpecification
   {
      protected IContainer _container;
      protected IContainer _liver;
      protected IContainer _cell;
      protected Observer _observer;
      protected Parameter _para1;
      protected Parameter _para2;

      protected override void Context()
      {
         _container = new Container();
         _liver = new Container {Name = "Liver"}.WithParentContainer(_container);
         _cell = new Container {Name = "Cell"}.WithParentContainer(_liver);
         _observer = new Observer {Name = "Obs"}.WithParentContainer(_cell);
         _para1 = new Parameter {Name = "Para1"}.WithParentContainer(_cell);
         _para2 = new Parameter {Name = "Para2"}.WithParentContainer(_container);
      }
   }

   public class When_setting_the_mode_of_a_container_with_the_extension : concern_for_ContainerExtensions
   {
      private IContainer _result;

      protected override void Because()
      {
         _result = _container.WithMode(ContainerMode.Physical);
      }

      [Observation]
      public void should_have_set_the_mode_in_the_container()
      {
         _container.Mode.ShouldBeEqualTo(ContainerMode.Physical);
      }

      [Observation]
      public void should_return_the_container()
      {
         _result.ShouldBeEqualTo(_container);
      }
   }

   public class When_resolving_all_sub_containers_fulfilling_a_given_conditions : concern_for_ContainerExtensions
   {
      private DescriptorCriteria _conditions;
      private IContainer _subContainerFulfilling;
      private IContainer _subContainerNotFulfilling;
      private IContainer _subSubContainerFulfilling;
      private IContainer _subSubContainerNotFulfilling;
      private IEnumerable<IContainer> _results;

      protected override void Context()
      {
         base.Context();
         _conditions = A.Fake<DescriptorCriteria>();
         _subContainerFulfilling = new Container().WithName("C1");
         _subContainerNotFulfilling = new Container().WithName("C2");
         _subSubContainerFulfilling = new Container().WithName("C3");
         _subSubContainerNotFulfilling = new Container().WithName("C4");
         _container.Add(_subContainerFulfilling);
         _container.Add(_subContainerNotFulfilling);
         _subContainerFulfilling.Add(_subSubContainerNotFulfilling);
         _subContainerNotFulfilling.Add(_subSubContainerFulfilling);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subContainerFulfilling)).Returns(true);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subContainerNotFulfilling)).Returns(false);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subSubContainerFulfilling)).Returns(true);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subSubContainerNotFulfilling)).Returns(false);
      }

      protected override void Because()
      {
         _results = new[] {_container, _subContainerFulfilling, _subSubContainerFulfilling, _subSubContainerNotFulfilling}.AllContainersFor(_conditions);
      }

      [Observation]
      public void should_return_only_the_sub_containers_whose_descriptors_match_the_condition()
      {
         _results.ShouldOnlyContain(_subContainerFulfilling, _subSubContainerFulfilling);
      }

      [Observation]
      public void should_not_return_the_sub_containers_whose_descriptors_do_not_match_the_condition()
      {
         _results.Contains(_subContainerNotFulfilling).ShouldBeFalse();
         _results.Contains(_subSubContainerNotFulfilling).ShouldBeFalse();
      }
   }

   public class When_resolving_all_containers_fulfilling_a_given_conditions : concern_for_ContainerExtensions
   {
      private DescriptorCriteria _conditions;
      private IContainer _subContainerFulfilling;
      private IContainer _subContainerNotFulfilling;
      private IEnumerable<IContainer> _results;

      protected override void Context()
      {
         base.Context();
         _conditions = A.Fake<DescriptorCriteria>();
         _subContainerFulfilling = new Container().WithName("C1");
         _subContainerNotFulfilling = new Container().WithName("C2");
         _container.Add(_subContainerFulfilling);
         _container.Add(_subContainerNotFulfilling);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subContainerFulfilling)).Returns(true);
         A.CallTo(() => _conditions.IsSatisfiedBy(_subContainerNotFulfilling)).Returns(false);
         A.CallTo(() => _conditions.IsSatisfiedBy(_container)).Returns(true);
      }

      protected override void Because()
      {
         _results = new[] {_subContainerFulfilling, _subContainerNotFulfilling, _container}.AllContainersFor(_conditions);
      }

      [Observation]
      public void should_return_only_containers_whose_descriptors_match_the_condition()
      {
         _results.ShouldOnlyContain(_subContainerFulfilling, _container);
      }
   }

   public class When_resolving_the_entity_defined_at_a_given_location : concern_for_ContainerExtensions
   {
      [Observation]
      public void should_return_the_expected_entity_when_the_entity_is_defined()
      {
         _container.EntityAt<Parameter>(_para2.Name).ShouldBeEqualTo(_para2);
         _container.EntityAt<Container>(_liver.Name).ShouldBeEqualTo(_liver);
         _container.EntityAt<Container>(_liver.Name, _cell.Name).ShouldBeEqualTo(_cell);
         _container.EntityAt<Observer>(_liver.Name, _cell.Name, _observer.Name).ShouldBeEqualTo(_observer);
         _container.EntityAt<Parameter>(_liver.Name, _cell.Name, _para1.Name).ShouldBeEqualTo(_para1);
      }

      [Observation]
      public void should_return_null_if_the_entity_is_not_defined()
      {
         _container.EntityAt<Parameter>(_liver.Name, _cell.Name, _observer.Name).ShouldBeNull();
         _container.EntityAt<Observer>(_liver.Name, _cell.Name, _para1.Name).ShouldBeNull();
         _container.EntityAt<Container>(_liver.Name, "toto").ShouldBeNull();
         _container.EntityAt<Container>().ShouldBeNull();
      }
   }
}