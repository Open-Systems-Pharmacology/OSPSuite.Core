using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace OSPSuite.Core
{
     public abstract class concern_for_CommandTask : ContextSpecification<ICommandTask>
   {
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         sut = new CommandTask();
      }
   }

   
   public class When_reset_all_changes_from_a_command : concern_for_CommandTask
   {
      private IReversibleCommand<IOSPSuiteExecutionContext> _commandToReset;
      private IReversibleCommand<IOSPSuiteExecutionContext> _inverseCommand;

      protected override void Context()
      {
         base.Context();
         _commandToReset = A.Fake<IReversibleCommand<IOSPSuiteExecutionContext>>();
         _inverseCommand = A.Fake<IReversibleCommand<IOSPSuiteExecutionContext>>();
         A.CallTo(() => _commandToReset.InverseCommand(_executionContext)).Returns(_inverseCommand);
      }

      protected override void Because()
      {
         sut.ResetChanges(_commandToReset, _executionContext);
      }

      [Observation]
      public void should_retreive_the_inverse_command_of_the_command_defined_as_parameter()
      {
         A.CallTo(() => _commandToReset.InverseCommand(_executionContext)).MustHaveHappened();
      }

      [Observation]
      public void should_retore_the_execution_data_for_the_command()
      {
         A.CallTo(() => _commandToReset.RestoreExecutionData(_executionContext)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_the_inverse_command()
      {
         A.CallTo(() => _inverseCommand.Execute(_executionContext)).MustHaveHappened();
      }
   }
}	