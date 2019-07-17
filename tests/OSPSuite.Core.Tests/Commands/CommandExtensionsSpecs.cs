using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public class When_calling_run_for_a_command_ : StaticContextSpecification
   {
      private IMyCommandWithProperties _typedCommand;
      private MyContext _context;
      private string _resultOfCall;
      private string _result;

      [Observation]
      public void should_leverage_the_execute_for_the_command()
      {
         A.CallTo(() => _typedCommand.Execute(_context)).MustHaveHappened();
      }

      [Observation]
      public void should_be_able_to_chain_the_run_with_a_call_to_a_function_from_the_typed_command()
      {
         _result.ShouldBeEqualTo(_resultOfCall);
      }

      protected override void Because()
      {
         _result = _typedCommand.Run(_context).OnePropertyToBeCalled();
      }

      protected override void Context()
      {
         _typedCommand = A.Fake<IMyCommandWithProperties>();
         _resultOfCall = "toto";
         A.CallTo(() => _typedCommand.OnePropertyToBeCalled()).Returns(_resultOfCall);
         _context = new MyContext();
      }
   }

   public class When_setting_the_inverse_properties_for_a_command_ : StaticContextSpecification
   {
      private CommandId _commandId;
      private IReversibleCommand<MyContext> _originalCommand;
      private IReversibleCommand<MyContext> _inverseCommand;

      [Observation]
      public void the_created_command_should_indeed_be_the_inverse_of_the_orginal_command()
      {
         _inverseCommand.IsInverseFor(_originalCommand).ShouldBeTrue();
      }

      [Observation]
      public void should_set_the_objec_type_of_the_newly_created_command_to_the_orginal_object_type()
      {
         _inverseCommand.ObjectType.ShouldBeEqualTo(_originalCommand.ObjectType);
      }

      protected override void Because()
      {
         _inverseCommand = new MyReversibleCommand().AsInverseFor(_originalCommand);
      }

      protected override void Context()
      {
         _commandId = new CommandId();
         _originalCommand = new MyReversibleCommand(_commandId);
      }
   }

   public class When_asked_in_a_command_is_empty_ : StaticContextSpecification
   {
      private ICommand _command1;
      private ICommand _command2;

      protected override void Context()
      {
         _command1 = new EmptyCommand<MyContext>();
         _command2 = A.Fake<ICommand>();
      }

      [Observation]
      public void should_return_true_if_the_command_is_an_empty_command()
      {
         _command1.IsEmpty().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_command_not_an_empty_command()
      {
         _command2.IsEmpty().ShouldBeFalse();
      }
   }

   public class When_asked_in_a_command_is_an_empty_macro : StaticContextSpecification
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;

      protected override void Context()
      {
         var macrocommand1 = new MacroCommand<MyContext>();
         macrocommand1.Add(A.Fake<ICommand<MyContext>>());
         _command1 = macrocommand1;
         _command2 = A.Fake<ICommand>();
         _command3 = new MacroCommand<MyContext>();
      }

      [Observation]
      public void should_return_false_if_the_command_is_a_macro_command_with_at_least_one_command()
      {
         _command1.IsEmptyMacro().ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_command_is_not_a_macro_command()
      {
         _command2.IsEmptyMacro().ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_command_is_an_empty_macro_command()
      {
         _command3.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public interface IMyCommandWithProperties : ICommand<MyContext>
   {
      string OnePropertyToBeCalled();
   }
}