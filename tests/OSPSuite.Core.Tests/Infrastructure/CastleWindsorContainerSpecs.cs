using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Castle.MicroKernel;
using Castle.Windsor;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Reflection;
using OSPSuite.Infrastructure.Container.Castle;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_CastleWindsorContainer : ContextSpecification<CastleWindsorContainer>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CastleWindsorContainer();
      }
   }

   public class When_asked_to_register_a_type_at_run_time : concern_for_CastleWindsorContainer
   {
      private IDbConnection _connection;

      protected override void Context()
      {
         base.Context();
         _connection = A.Fake<IDbConnection>();
      }

      protected override void Because()
      {
         sut.RegisterImplementationOf(_connection);
      }

      [Observation]
      public void should_return_the_implementation_that_was_registered()
      {
         sut.Resolve<IDbConnection>().ShouldBeEqualTo(_connection);
      }
   }

   public class When_asked_to_register_a_type_at_run_time_with_key : concern_for_CastleWindsorContainer
   {
      private IDbConnection _connection;

      protected override void Context()
      {
         base.Context();
         _connection = A.Fake<IDbConnection>();
      }

      protected override void Because()
      {
         sut.RegisterImplementationOf(_connection, "toto");
      }

      [Observation]
      public void should_return_the_implementation_that_was_registered()
      {
         sut.Resolve<IDbConnection>().ShouldBeEqualTo(_connection);
      }

      [Observation]
      public void should_return_the_implementation_that_was_registered_when_called_with_the_key()
      {
         sut.Resolve<IDbConnection>("toto").ShouldBeEqualTo(_connection);
      }
   }

   public class When_registering_the_same_type_twice_with_different_keys : concern_for_CastleWindsorContainer
   {
      private IDbConnection _connection1;
      private IDbConnection _connection2;

      protected override void Context()
      {
         base.Context();
         _connection1 = A.Fake<IDbConnection>();
         _connection2 = A.Fake<IDbConnection>();
      }

      protected override void Because()
      {
         sut.RegisterImplementationOf(_connection1, "toto");
         sut.RegisterImplementationOf(_connection2, "tata");
      }

      [Observation]
      public void should_return_the_correct_implementation_that_was_registered_with_key()
      {
         sut.Resolve<IDbConnection>("toto").ShouldBeEqualTo(_connection1);
         sut.Resolve<IDbConnection>("tata").ShouldBeEqualTo(_connection2);
      }

      [Observation]
      public void should_return_the_first_implementation_if_we_do_not_provide_a_key()
      {
         sut.Resolve<IDbConnection>().ShouldBeEqualTo(_connection1);
      }
   }

   public class When_resolving_a_type_that_was_registered_as_a_singleton : concern_for_CastleWindsorContainer
   {
      private IEnumerable<string> _result1;
      private IEnumerable<string> _result2;

      protected override void Context()
      {
         base.Context();
         sut.Register<IEnumerable<string>, List<string>>(LifeStyle.Singleton);
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IEnumerable<string>>();
         _result2 = sut.Resolve<IEnumerable<string>>();
      }

      [Observation]
      public void should_always_return_the_same_instance()
      {
         _result1.ShouldBeEqualTo(_result2);
      }
   }

   public class When_resolving_a_type_that_was_registered_with_a_key : concern_for_CastleWindsorContainer
   {
      private IAnInterface _result1;

      protected override void Context()
      {
         base.Context();
         sut.Register<IAnInterface, AnImplementation>("toto");
         sut.Register<IAnInterface, AnotherImplementation>("tutu");
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IAnInterface>("toto");
      }

      [Observation]
      public void should_return_the_accurate_object_type_for_the_interface()
      {
         _result1.ShouldBeAnInstanceOf<AnImplementation>();
      }
   }

   public class When_resolving_a_an_entity_by_type : concern_for_CastleWindsorContainer
   {
      private IAnInterface _result1;

      protected override void Context()
      {
         base.Context();
         sut.Register<IAnInterface, AnImplementation>();
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IAnInterface>(typeof(IAnInterface));
      }

      [Observation]
      public void should_return_the_accurate_object_type_for_the_interface()
      {
         _result1.ShouldBeAnInstanceOf<AnImplementation>();
      }
   }

   public class When_resolving_a_type_that_was_registered_as_a_singleton_and_with_a_key : concern_for_CastleWindsorContainer
   {
      private IEnumerable<string> _result1;
      private IEnumerable<string> _result2;

      protected override void Context()
      {
         base.Context();
         sut.Register<IEnumerable<string>, List<string>>(LifeStyle.Singleton, "toto");
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IEnumerable<string>>("toto");
         _result2 = sut.Resolve<IEnumerable<string>>("toto");
      }

      [Observation]
      public void should_always_return_a_new_instace()
      {
         _result1.ShouldBeEqualTo(_result2);
      }
   }

   public class When_resolving_a_type_that_was_registered_as_a_transient : concern_for_CastleWindsorContainer
   {
      private IDbCommand _result1;
      private IDbCommand _result2;

      protected override void Context()
      {
         base.Context();
         sut.Register<IDbCommand, OdbcCommand>(LifeStyle.Transient);
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IDbCommand>();
         _result2 = sut.Resolve<IDbCommand>();
      }

      [Observation]
      public void should_always_return_a_new_instace()
      {
         _result1.ShouldNotBeEqualTo(_result2);
      }
   }

   public class When_resolving_a_type_that_was_registered_as_a_transient_and_with_a_key : concern_for_CastleWindsorContainer
   {
      private IDbCommand _result1;
      private IDbCommand _result2;

      protected override void Context()
      {
         base.Context();
         sut.Register<IDbCommand, OdbcCommand>(LifeStyle.Transient, "toto");
      }

      protected override void Because()
      {
         _result1 = sut.Resolve<IDbCommand>("toto");
         _result2 = sut.Resolve<IDbCommand>("toto");
      }

      [Observation]
      public void should_always_return_a_new_instace()
      {
         _result1.ShouldNotBeEqualTo(_result2);
      }
   }

   public class When_asked_to_resolve_all_implementation : concern_for_CastleWindsorContainer
   {
      private IDbConnection _connection1;
      private IEnumerable<IDbConnection> _result;
      private IDbConnection _connection2;

      protected override void Context()
      {
         base.Context();
         _connection1 = A.Fake<IDbConnection>();
         _connection2 = A.Fake<IDbConnection>();
         sut.RegisterImplementationOf(_connection1, "1");
         sut.RegisterImplementationOf(_connection2, "2");
      }

      protected override void Because()
      {
         _result = sut.ResolveAll<IDbConnection>();
      }

      [Observation]
      public void should_leverage_the_underlying_container_to_resolve_the_implementation()
      {
         _result.ShouldOnlyContain(_connection1, _connection2);
      }
   }

   public class When_calling_the_dispose_method_on_the_container : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = A.Fake<IWindsorContainer>();
         sut = new CastleWindsorContainer(_underlyingContainer);
      }

      protected override void Because()
      {
         sut.Dispose();
      }

      [Observation]
      public void should_dispose_the_container()
      {
         A.CallTo(() => _underlyingContainer.Dispose()).MustHaveHappened();
      }
   }

   public class When_resolving_an_object_with_an_interface_for_which_only_an_object_with_the_base_interface_was_registered : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = new WindsorContainer();
         sut = new CastleWindsorContainer(_underlyingContainer);
         sut.Register<IAnInterface, AnImplementation>();
      }

      [Observation]
      public void should_not_be_able_to_retrieve_an_implementation_from_the_base_interface()
      {
         The.Action(()=> sut.Resolve<INotifier>()).ShouldThrowAn<ComponentNotFoundException>();
      }
   }

   public class When_resolving_an_object_with_an_that_was_not_registered : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = new WindsorContainer();
         sut = new CastleWindsorContainer(_underlyingContainer);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Resolve<INotifier>()).ShouldThrowAn<ComponentNotFoundException>();
      }
   }

   public class When_the_setting_injection_is_disabled : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = new WindsorContainer();
         sut = new CastleWindsorContainer(_underlyingContainer);
         sut.Register<ISubObject, SubObject>();
         sut.Register<IOneObject, OneObject>();
      }

      [Observation]
      public void should_not_resolve_properties()
      {
         sut.Resolve<IOneObject>().SubObject.ShouldBeNull();
      }
   }

   public class When_the_setting_injection_is_disabled_by_default : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = new WindsorContainer();
         sut = new CastleWindsorContainer(_underlyingContainer);
         sut.Register<ISubObject, SubObject>();
         sut.Register<IOneObject, OneObject>();
      }

      [Observation]
      public void should_not_resolve_properties()
      {
         sut.Resolve<IOneObject>().SubObject.ShouldBeNull();
      }
   }

   public class When_the_setting_injection_is_enabled : concern_for_CastleWindsorContainer
   {
      private IWindsorContainer _underlyingContainer;

      protected override void Context()
      {
         _underlyingContainer = new WindsorContainer();
         sut = new CastleWindsorContainer(_underlyingContainer, new WindsorLifeStyleMapper(), true);
         sut.Register<ISubObject, SubObject>();
         sut.Register<IOneObject, OneObject>();
      }

      [Observation]
      public void should_not_resolve_properties()
      {
         sut.Resolve<IOneObject>().SubObject.ShouldNotBeNull();
      }
   }

   public class When_registering_an_object_using_multiple_interfaces_as_transient : concern_for_CastleWindsorContainer
   {
      protected override void Context()
      {
         base.Context();
         sut.Register<IAnInterface, INotifier, AnImplementation, AnImplementation>(LifeStyle.Transient);
      }

      [Observation]
      public void should_be_able_to_return_different_implementations_with_differnt_interfaces()
      {
         var imp1 = sut.Resolve<IAnInterface>();
         var imp2 = sut.Resolve<INotifier>();
         var imp3 = sut.Resolve<IAnInterface>();
         var imp4 = sut.Resolve<AnImplementation>();

         imp1.ShouldNotBeEqualTo(imp2);
         imp1.ShouldNotBeEqualTo(imp3);
         imp2.ShouldNotBeEqualTo(imp3);
         imp3.ShouldNotBeEqualTo(imp4);
      }
   }

   public class When_registering_an_object_using_multiple_interfaces_as_singleton : concern_for_CastleWindsorContainer
   {
      protected override void Context()
      {
         base.Context();
         sut.Register<IAnInterface, INotifier, AnImplementation>(LifeStyle.Singleton);
      }

      [Observation]
      public void should_be_able_to_return_different_implementations_with_differnt_interfaces()
      {
         var imp1 = sut.Resolve<IAnInterface>();
         var imp2 = sut.Resolve<INotifier>();
         var imp3 = sut.Resolve<IAnInterface>();

         imp1.ShouldBeEqualTo(imp2);
         imp1.ShouldBeEqualTo(imp3);
         imp2.ShouldBeEqualTo(imp3);
      }
   }

   public interface IOneObject
   {
      ISubObject SubObject { get; set; }
   }

   public class OneObject : IOneObject
   {
      public ISubObject SubObject { get; set; }
   }

   public interface ISubObject
   {
   }

   public class SubObject : ISubObject
   {
   }

   public interface IAnInterface : INotifier
   {
   
   }

   public class AnImplementation : Notifier,  IAnInterface
   {
      
   }

   public class AnotherImplementation : Notifier, IAnInterface
   {
      
   }
}