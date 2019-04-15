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