using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectPathExtensions : StaticContextSpecification
   {
      protected IObjectPath _objectPath;
      protected string _entryToAdd;

      protected override void Context()
      {
         _entryToAdd = "toto";
         _objectPath = A.Fake<IObjectPath>();
      }
   }

   public class When_adding_a_path_entry_to_an_object_path_with_an_extension : concern_for_ObjectPathExtensions
   {
      protected override void Because()
      {
         _objectPath.AndAdd(_entryToAdd);
      }

      [Observation]
      public void should_add_the_path_entry_at_the_end_of_the_path()
      {
         A.CallTo(() => _objectPath.Add(_entryToAdd)).MustHaveHappened();
      }
   }

   public class When_adding_a_path_entry_at_the_beginning_of_an_object_path_with_an_extension : concern_for_ObjectPathExtensions
   {
      protected override void Because()
      {
         _objectPath.AndAddAtFront(_entryToAdd);
      }

      [Observation]
      public void should_add_the_path_entry_at_the_front_of_the_path()
      {
         A.CallTo(() => _objectPath.AddAtFront(_entryToAdd)).MustHaveHappened();
      }
   }

   
   public class When_resolving_an_entity_that_can_be_found_with_the_try_method_ : concern_for_ObjectPathExtensions
   {
      private IEntity _refEntity;
      private bool _success;
      private IEntity _resolvedEntity;
      private IEntity _result;

      protected override void Context()
      {
         base.Context();
         _refEntity =A.Fake<IEntity>();
         _resolvedEntity =A.Fake<IEntity>();
         A.CallTo(() => _objectPath.Resolve<IEntity>(_refEntity)).Returns(_resolvedEntity);
      }
      protected override void Because()
      {
         _result =_objectPath.TryResolve<IEntity>(_refEntity, out _success);
      }
      [Observation]
      public void should_set_the_result_flag_to_true_and_return_the_entity()
      {
         _result.ShouldBeEqualTo(_resolvedEntity);
         _success.ShouldBeTrue();
      }
   }

   
   public class When_resolving_an_entity_that_cannot_be_found_with_the_try_method_and_success_flag : concern_for_ObjectPathExtensions
   {
      private IEntity _refEntity;
      private bool _success;
      private IEntity _result;

      protected override void Context()
      {
         base.Context();
         _refEntity = A.Fake<IEntity>();
        A.CallTo(()=> _objectPath.Resolve<IEntity>(_refEntity)).Throws(new Exception());
      }
      protected override void Because()
      {
         _result = _objectPath.TryResolve<IEntity>(_refEntity, out _success);
      }

      [Observation]
      public void should_set_the_result_flag_to_false_and_return_null()
      {
         _result.ShouldBeNull();
         _success.ShouldBeFalse();
      }
   }

   public class When_resolving_an_entity_that_cannot_be_found_with_the_try_method : concern_for_ObjectPathExtensions
   {
      private IEntity _refEntity;

      protected override void Context()
      {
         base.Context();
         _refEntity = A.Fake<IEntity>();
         A.CallTo(() => _objectPath.Resolve<IEntity>(_refEntity)).Throws(new Exception());
      }
  
      [Observation]
      public void should_return_null()
      {
         _objectPath.TryResolve<IEntity>(_refEntity).ShouldBeNull();
      }
   }
}