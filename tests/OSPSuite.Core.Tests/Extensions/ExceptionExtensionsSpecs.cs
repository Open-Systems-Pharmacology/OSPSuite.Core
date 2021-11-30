using System;
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
}