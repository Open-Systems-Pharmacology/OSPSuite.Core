using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ClearHistoryUICommand : ContextSpecification<ClearHistoryUICommand>
   {
      protected IHistoryTask _historyTask;

      protected override void Context()
      {
         _historyTask= A.Fake<IHistoryTask>();
         sut = new ClearHistoryUICommand(_historyTask);
      }
   }

   public class When_executing_the_clear_history_command : concern_for_ClearHistoryUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_history_task_command_to_clear_the_history()
      {
         A.CallTo(() => _historyTask.ClearHistory()).MustHaveHappened();   
      }
   }
}	