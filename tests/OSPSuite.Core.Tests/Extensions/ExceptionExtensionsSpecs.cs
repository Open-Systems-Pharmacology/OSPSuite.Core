using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Extensions
{
   public abstract class concern_for_ExceptionExtensions : StaticContextSpecification
   {
      private IContainer _container;
      private IContainer _oldContainer;
      protected IExceptionManager _exceptionManager;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _container = A.Fake<IContainer>();
         _exceptionManager = A.Fake<IExceptionManager>();
         A.CallTo(() => _container.Resolve<IExceptionManager>()).Returns(_exceptionManager);
         _oldContainer = IoC.Container;
         IoC.InitializeWith(_container);
      }

      protected Task CreateTaskThrowingException()
      {
         return Task.Run(() => throw new OSPSuiteException("ERROR"));
      }

      protected Task<int> CreateTaskWithReturnValueThrowingException()
      {
         return Task.Run(() =>
         {
            if (true)
               throw new OSPSuiteException("ERROR");

#pragma warning disable 162
            return 5;
#pragma warning restore 162
         });
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         IoC.InitializeWith(_oldContainer);
      }
   }

   public class When_executing_and_async_action_throwing_an_exception : concern_for_ExceptionExtensions
   {
      [Observation]
      public async Task should_be_able_to_catch_the_exception()
      {
         Func<Task> fnc = CreateTaskThrowingException;
         await fnc.DoWithinExceptionHandler();
         A.CallTo(() => _exceptionManager.LogException(A<OSPSuiteException>._)).MustHaveHappened();
      }
   }

   public class When_executing_and_async_action_with_result_throwing_an_exception : concern_for_ExceptionExtensions
   {
      [Observation]
      public async Task should_be_able_to_catch_the_exception()
      {
         Func<Task<int>> fnc = CreateTaskWithReturnValueThrowingException;
         await fnc.DoWithinExceptionHandler();
         A.CallTo(() => _exceptionManager.LogException(A<OSPSuiteException>._)).MustHaveHappened();
      }
   }

   public class When_executing_and_async_action_with_result_not_throwing_an_exception : concern_for_ExceptionExtensions
   {
      [Observation]
      public async Task should_return_the_results()
      {
         Func<Task<int>> fnc = () => Task.Run(() => 5);
         var res = await fnc.DoWithinExceptionHandler();
         res.ShouldBeEqualTo(5);
      }
   }

   public class When_checking_if_an_exception_is_an_out_of_memory : StaticContextSpecification
   {
      [Observation]
      public void should_detect_a_direct_out_of_memory()
      {
         new OutOfMemoryException().IsOutOfMemory().ShouldBeTrue();
      }

      [Observation]
      public void should_detect_an_out_of_memory_wrapped_in_an_inner_exception_chain()
      {
         new Exception("outer", new Exception("inner", new OutOfMemoryException())).IsOutOfMemory().ShouldBeTrue();
      }

      [Observation]
      public void should_detect_an_out_of_memory_nested_in_an_aggregate_exception()
      {
         new AggregateException(new Exception(), new AggregateException(new OutOfMemoryException())).IsOutOfMemory().ShouldBeTrue();
      }

      //the realistic shape of an out-of-memory under a fan-out: one worker exhausts memory, many siblings
      //fail too; every inner exception is inspected regardless of how wide the aggregate is
      [Observation]
      public void should_detect_an_out_of_memory_behind_many_sibling_failures()
      {
         var siblings = Enumerable.Range(0, 64).Select(i => (Exception) new Exception($"worker {i}", new Exception())).ToList();
         siblings.Add(new OutOfMemoryException());
         new AggregateException(siblings).IsOutOfMemory().ShouldBeTrue();
      }

      [Observation]
      public void should_not_detect_an_unrelated_exception()
      {
         new InvalidOperationException("boom", new Exception()).IsOutOfMemory().ShouldBeFalse();
      }

      [Observation]
      public void should_not_detect_a_null_exception()
      {
         ((Exception) null).IsOutOfMemory().ShouldBeFalse();
      }

      //exception chains from interop or serialization can be self-referencing; the walk is called from
      //exception filters where a stack overflow would be unrecoverable, so cycles must be detected
      [Observation]
      public void should_terminate_on_a_cyclic_inner_exception_chain()
      {
         //the inner exception is only settable at construction; the cycle requires the runtime's private field
         var innerExceptionField = typeof(Exception).GetField("_innerException", BindingFlags.Instance | BindingFlags.NonPublic);
         innerExceptionField.ShouldNotBeNull();

         var first = new Exception("first");
         var second = new Exception("second", first);
         innerExceptionField.SetValue(first, second);

         second.IsOutOfMemory().ShouldBeFalse();
      }
   }
}