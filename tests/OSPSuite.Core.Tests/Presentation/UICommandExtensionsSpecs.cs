using OSPSuite.BDDHelper;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Presentation.Events;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_UICommandExtensions : StaticContextSpecification
   {
      protected IUICommand _command;
      protected IMainViewPresenter _mainViewPresenter;
      protected IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _mainViewPresenter = A.Fake<IMainViewPresenter>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _command = A.Fake<IUICommand>();
      }
   }

   public class When_executing_a_command_with_the_command_extensions : concern_for_UICommandExtensions
   {
      protected override void Because()
      {
         _command.ExecuteWithinExceptionHandler(_eventPublisher, _mainViewPresenter);
      }

      [Observation]
      public void should_encapsulate_the_execution_of_the_command_between_the_start_and_finish_heavy_work_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<HeavyWorkStartedEvent>._)).MustHaveHappened();
         A.CallTo(() => _eventPublisher.PublishEvent(A<HeavyWorkFinishedEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void should_change_the_model()
      {
         A.CallTo(() => _mainViewPresenter.SaveChanges()).MustHaveHappened();
      }

      [Observation]
      public void should_execute_the_command()
      {
         A.CallTo(() => _command.Execute()).MustHaveHappened();
      }
   }
}