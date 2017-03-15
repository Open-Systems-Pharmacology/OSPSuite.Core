using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Exceptions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_HeavyWorkManager : ContextSpecification<IHeavyWorkManager>
   {
      protected IHeavyWorkPresenterFactory _heavyWorkPresenterFactory;
      protected IExceptionManager _exceptionManager;
      protected IHeavyWorkPresenter _heavyWorkPresenter;

      protected override void Context()
      {
         _heavyWorkPresenterFactory = A.Fake<IHeavyWorkPresenterFactory>();
         _heavyWorkPresenter = A.Fake<IHeavyWorkPresenter>();
         A.CallTo(() => _heavyWorkPresenterFactory.Create()).Returns(_heavyWorkPresenter);
         _exceptionManager = A.Fake<IExceptionManager>();
         sut = new HeavyWorkManager(_heavyWorkPresenterFactory, _exceptionManager);
      }
   }

   public class When_the_heavy_work_manager_is_starting_an_action : concern_for_HeavyWorkManager
   {
      private Action _action;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _action = () => { };

      }
      protected override void Because()
      {
         _result = sut.Start(_action);
      }

      [Observation]
      public void should_retrieve_a_new_heavy_work_presenter_and_start_it()
      {
         A.CallTo(() => _heavyWorkPresenter.Start(Captions.PleaseWait)).MustHaveHappened();
      }


      [Observation]
      public void should_true_if_the_action_was_successfull_otherwise_false()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_dispose_of_the_presenter_when_the_action_is_completed()
      {
         A.CallTo(() => _heavyWorkPresenter.Dispose()).MustHaveHappened();
      }
   }

   public class When_the_heavy_work_manager_is_starting_an_action_with_a_predefined_caption : concern_for_HeavyWorkManager
   {
      private Action _action;
      private string _caption;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _caption = "traa";
         _action = () => { };

      }
      protected override void Because()
      {
         _result = sut.Start(_action, _caption);
      }

      [Observation]
      public void should_retrieve_a_new_heavy_work_presenter_and_start_it_with_the_given_the_caption()
      {
         A.CallTo(() => _heavyWorkPresenter.Start(_caption)).MustHaveHappened();
      }

      [Observation]
      public void should_true_if_the_action_was_successfull_otherwise_false()
      {
         _result.ShouldBeEqualTo(true);
      }

      [Observation]
      public void should_dispose_of_the_presenter_when_the_action_is_completed()
      {
         A.CallTo(() => _heavyWorkPresenter.Dispose()).MustHaveHappened();
      }

   }

   public class the_heavy_work_manager_is_executing_an_action_that_throws_an_exception : concern_for_HeavyWorkManager
   {
      private Exception _exception;
      private Action _action;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _exception = new Exception("bla");
         _action = () =>
         {
            throw _exception;
         };
      }

      protected override void Because()
      {
         _result = sut.Start(_action);
      }

      [Observation]
      public void should_log_the_exception()
      {
         if (!_result)
            A.CallTo(() => _exceptionManager.LogException(_exception)).MustHaveHappened();
      }

      [Observation]
      public void should_dispose_of_the_presenter_when_the_action_is_completed()
      {
         A.CallTo(() => _heavyWorkPresenter.Dispose()).MustHaveHappened();
      }
   }
}