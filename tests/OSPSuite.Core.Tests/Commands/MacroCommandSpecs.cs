using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;
using MacroCommandExtensions = OSPSuite.Core.Commands.Core.MacroCommandExtensions;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_MacroCommand : ContextSpecification<IMacroCommand<MyContext>>
   {
      protected IList<ICommand<MyContext>> _listOfAvailableCommands;
      protected MyContext _context;

      protected override void Context()
      {
         _listOfAvailableCommands = new List<ICommand<MyContext>>();
         sut = new MacroCommand<MyContext>(_listOfAvailableCommands);
         _context = new MyContext();
      }
   }

   public class When_adding_a_command : concern_for_MacroCommand
   {
      private ICommand<MyContext> _commandToAdd;

      protected override void Context()
      {
         base.Context();
         _commandToAdd = A.Fake<ICommand<MyContext>>();
      }

      protected override void Because()
      {
         sut.Add(_commandToAdd);
      }

      [Observation]
      public void the_macro_command_should_not_be_empty()
      {
         sut.IsEmtpy.ShouldBeFalse();
      }

      [Observation]
      public void should_add_the_command_to_the_underlying_list_of_commands_for_that_macro()
      {
         _listOfAvailableCommands.Contains(_commandToAdd).ShouldBeTrue();
      }
   }

   public class When_asked_for_all_sub_commands : concern_for_MacroCommand
   {
      private ICommand<MyContext> _commandToAdd1;
      private ICommand<MyContext> _commandToAdd2;
      private IList<ICommand> result;

      protected override void Context()
      {
         base.Context();
         _commandToAdd1 = A.Fake<ICommand<MyContext>>();
         _commandToAdd2 = A.Fake<ICommand<MyContext>>();
         MacroCommandExtensions.Add(sut, _commandToAdd1, _commandToAdd2);
      }

      protected override void Because()
      {
         result = sut.All().ToList();
      }

      [Observation]
      public void should_yield_the_sub_commands_it_contains()
      {
         result.ShouldOnlyContainInOrder(_commandToAdd1, _commandToAdd2);
      }
   }

   public class When_retrieving_the_inverse_command_for_a_macro_command_containing_only_reversible_commands :
      concern_for_MacroCommand
   {
      private IMacroCommand<MyContext> _result;
      private IReversibleCommand<MyContext> _reversibleCommand1;
      private IReversibleCommand<MyContext> _reversibleCommand2;
      private IReversibleCommand<MyContext> _reversibleCommand3;

      protected override void Context()
      {
         base.Context();
         _reversibleCommand1 = new MyReversibleCommand();
         _reversibleCommand2 = new MyReversibleCommand();
         _reversibleCommand3 = new MyReversibleCommand();

         MacroCommandExtensions.Add(sut, _reversibleCommand1, _reversibleCommand2, _reversibleCommand3);
      }

      protected override void Because()
      {
         _result = sut.InverseCommand(_context) as IMacroCommand<MyContext>;
      }

      [Observation]
      public void should_return_a_new_macro_command()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void the_returned_macro_command_should_contain_all_inverse_sub_command_in_the_accurate_order()
      {
         var subCommands = _result.All().ToList();
         subCommands.Count.ShouldBeEqualTo(3);
         subCommands[0].Id.IsInverseFor(_reversibleCommand3.Id).ShouldBeTrue();
         subCommands[1].Id.IsInverseFor(_reversibleCommand2.Id).ShouldBeTrue();
         subCommands[2].Id.IsInverseFor(_reversibleCommand1.Id).ShouldBeTrue();
      }
   }

   public class When_retrieving_the_inverse_command_for_a_macro_command_containg_non_reversible_command : concern_for_MacroCommand
   {
      protected override void Context()
      {
         base.Context();
         MacroCommandExtensions.Add(sut, A.Fake<IReversibleCommand<MyContext>>(), A.Fake<ICommand<MyContext>>());
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.InverseCommand(_context)).ShouldThrowAn<CreateInverseCommandException>();
      }
   }

   public class When_executing_a_macro_command : concern_for_MacroCommand
   {
      private ICommand<MyContext> _commandToExecute1;
      private ICommand<MyContext> _commandToExecute2;
      private ICommand<MyContext> _commandToExecute3;

      [Observation]
      public void should_execute_all_sub_commands_with_the_accurate_context()
      {
         A.CallTo(() => _commandToExecute1.Execute(_context)).MustHaveHappened();
         A.CallTo(() => _commandToExecute2.Execute(_context)).MustHaveHappened();
         A.CallTo(() => _commandToExecute3.Execute(_context)).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      protected override void Context()
      {
         base.Context();
         _commandToExecute1 = A.Fake<ICommand<MyContext>>();
         _commandToExecute2 = A.Fake<ICommand<MyContext>>();
         _commandToExecute3 = A.Fake<ICommand<MyContext>>();

         MacroCommandExtensions.Add(sut, _commandToExecute1, _commandToExecute2, _commandToExecute3);

         A.CallTo(() => _commandToExecute1.Id).Returns(new CommandId());
         A.CallTo(() => _commandToExecute2.Id).Returns(new CommandId());
         A.CallTo(() => _commandToExecute3.Id).Returns(new CommandId());
      }
   }

   public class When_restoring_the_execution_data_for_a_macro_command : concern_for_MacroCommand
   {
      private IReversibleCommand<MyContext> _commandToExecute1;
      private IReversibleCommand<MyContext> _commandToExecute2;
      private IReversibleCommand<MyContext> _commandToExecute3;

      protected override void Context()
      {
         base.Context();
         _commandToExecute1 = A.Fake<IReversibleCommand<MyContext>>();
         _commandToExecute2 = A.Fake<IReversibleCommand<MyContext>>();
         _commandToExecute3 = A.Fake<IReversibleCommand<MyContext>>();

         MacroCommandExtensions.Add(sut, _commandToExecute1, _commandToExecute2, _commandToExecute3);
         A.CallTo(() => _commandToExecute1.Id).Returns(new CommandId());
         A.CallTo(() => _commandToExecute2.Id).Returns(new CommandId());
         A.CallTo(() => _commandToExecute3.Id).Returns(new CommandId());
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_restore_the_execution_data_for_all_sub_commands_with_the_accurate_context()
      {
         A.CallTo(() => _commandToExecute1.RestoreExecutionData(_context)).MustHaveHappened();
         A.CallTo(() => _commandToExecute2.RestoreExecutionData(_context)).MustHaveHappened();
         A.CallTo(() => _commandToExecute3.RestoreExecutionData(_context)).MustHaveHappened();
      }
   }

   public class When_clearing_the_content_of_the_macro_command : concern_for_MacroCommand
   {
      protected override void Context()
      {
         base.Context();
         sut.Add((ICommand) new MySimpleCommand());
         sut.Add((ICommand) new MySimpleCommand());
         sut.Add((ICommand) new MySimpleCommand());
         sut.Add((ICommand) new MySimpleCommand());
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void it_should_not_contain_any_sub_command()
      {
         _listOfAvailableCommands.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_an_empty_command_is_added_to_the_macro_command : concern_for_MacroCommand
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(new EmptyCommand<MyContext>());
      }

      [Observation]
      public void the_comamnd_should_not_have_been_registered_into_the_list_of_sub_commands()
      {
         _listOfAvailableCommands.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_an_empty_macro_command_is_added_to_the_macro_command : concern_for_MacroCommand
   {
      protected override void Context()
      {
         base.Context();
         var macroCommand = new MacroCommand<MyContext>();
         sut.Add(macroCommand);
      }

      [Observation]
      public void the_comamnd_should_not_have_been_registered_into_the_list_of_sub_commands()
      {
         _listOfAvailableCommands.Count.ShouldBeEqualTo(0);
      }
   }
}