using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_WithIdRepository : ContextSpecification<IWithIdRepository>
   {
      protected override void Context()
      {
         sut = new WithIdRepository();
      }
   }

   public class When_registering_an_object_base : concern_for_WithIdRepository
   {
      private IWithId _withId;

      protected override void Context()
      {
         base.Context();
         _withId = A.Fake<IWithId>().WithId("BLA");
      }

      protected override void Because()
      {
         sut.Register(_withId);
      }

      [Observation]
      public void should_contain_object_base()
      {
         sut.ContainsObjectWithId(_withId.Id).ShouldBeTrue();
      }
   }

   public class When_registering_an_object_base_for_which_the_id_is_not_set : concern_for_WithIdRepository
   {
      private IWithId _withId;

      protected override void Context()
      {
         base.Context();
         _withId = A.Fake<IWithId>().WithId("");
      }

      protected override void Because()
      {
         sut.Register(_withId);
      }

  
      [Observation]
      public void should_not_register_the_object()
      {
         sut.ContainsObjectWithId(_withId.Id).ShouldBeFalse();
      }
   }

   public class When_retrieving_a_registerd_object : concern_for_WithIdRepository
   {
      private IEntity _entity;
      private IEntity _result;

      protected override void Context()
      {
         base.Context();
         _entity = A.Fake<IEntity>().WithId("LALA");
         sut.Register(_entity);
      }

      protected override void Because()
      {
         _result = sut.Get<IEntity>(_entity.Id);
      }

      [Observation]
      public void should_return_the_entity()
      {
         _result.ShouldBeEqualTo(_entity);
      }
   }

   public class When_retrieving_an_unregisterd_object : concern_for_WithIdRepository
   {
      protected override void Context()
      {
         base.Context();
         var withId = A.Fake<IWithId>().WithId("BLA");
         sut.Register(withId);
      }

      [Observation]
      public void should_return_null()
      {
         sut.Get("Not Present").ShouldBeNull();
      }
   }

   public class When_getting_an_registerd_object_with_a_wrong_type : concern_for_WithIdRepository
   {
      protected override void Context()
      {
         base.Context();
         var withId = A.Fake<IEntity>().WithId("BLA");
         sut.Register(withId);
      }

      [Observation]
      public void should_return_null()
      {
         sut.Get<IContainer>("BLA").ShouldBeNull();
      }
   }

   public class When_registering_an_object_with_an_id_that_was_already_registered : concern_for_WithIdRepository
   {
      private IEntity _entity;
      //register the same entity two times should cause an Exception
      protected override void Context()
      {
         base.Context();
         _entity = A.Fake<IEntity>();
         _entity.Id = "LALA";
         sut.Register(_entity);
      }

      [Observation]
      public void should_throw_an_not_unique_id_exception()
      {
         The.Action(() => sut.Register(A.Fake<IEntity>().WithId(_entity.Id))).ShouldThrowAn<NotUniqueIdException>();
      }
   }

   public class When_registering_the_same_object_twice : concern_for_WithIdRepository
   {
      private IEntity _entity;
      //register the same entity two times should cause an Exception
      protected override void Context()
      {
         base.Context();
         _entity = A.Fake<IEntity>();
         _entity.Id = "LALA";
         sut.Register(_entity);
      }

      [Observation]
      public void should_not_throw_an_exception()
      {
         sut.Register(_entity);
      }
   }

   public class When_unregistering_an_object : concern_for_WithIdRepository
   {
      private IEntity _entity;

      protected override void Context()
      {
         base.Context();

         _entity = A.Fake<IEntity>();
         _entity.Id = "LALA";
         sut.Register(_entity);
      }

      protected override void Because()
      {
         sut.Unregister(_entity.Id);
      }

      [Observation]
      public void should_no_longer_the_object()
      {
         sut.ContainsObjectWithId(_entity.Id).ShouldBeFalse();
      }
   }
}