using System.Collections.Generic;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_CommandsExpander : ContextSpecification<ICommandsExpander>
   {
      protected override void Context()
      {
         sut = new CommandsExpander();
      }
   }

   public class When_retrieving_the_list_of_expanded_id_from_a_set_of_commands : concern_for_CommandsExpander
   {
      private readonly ICommand<MyContext> _simpleCommand1 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand2 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand3 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand4 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand5 = new MyReversibleCommand();
      private IList<ICommand> _commandList;
      private IEnumerable<ICommand> _result;

      protected override void Context()
      {
         base.Context();
         //Create 3 commands. The second one is a macrocommand that contains 2 commands. The 2 commands is also a macro comamnd with 2 simple comamnd
         _commandList = new List<ICommand>();

         _commandList.Add(_simpleCommand1);

         var macroCommand1 = new MacroCommand<MyContext>();
         macroCommand1.Add(_simpleCommand2);

         var macroCommand2 = new MacroCommand<MyContext>();
         macroCommand2.Add(_simpleCommand3, _simpleCommand4);

         macroCommand1.Add(macroCommand2);

         _commandList.Add(macroCommand1);

         _commandList.Add(_simpleCommand5);
      }

      protected override void Because()
      {
         _result = sut.Expands(_commandList);
      }

      [Observation]
      public void should_return_the_flatten_list_of_ids()
      {
         _result.ShouldOnlyContainInOrder(_simpleCommand1, _simpleCommand2, _simpleCommand3, _simpleCommand4, _simpleCommand5);
      }
   }

   public class When_expending_only_one_command : concern_for_CommandsExpander
   {
      private readonly ICommand<MyContext> _simpleCommand1 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand2 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand3 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand4 = new MyReversibleCommand();
      private readonly ICommand<MyContext> _simpleCommand5 = new MyReversibleCommand();
      private IEnumerable<ICommand> _result;
      private MacroCommand<MyContext> _containerCommand;
      private MacroCommand<MyContext> _macroCommand1;
      private MacroCommand<MyContext> _macroCommand2;

      protected override void Context()
      {
         base.Context();
         //Create 3 commands. The second one is a macrocommand that contains 2 commands. The 2 commands is also a macro comamnd with 2 simple comamnd
         _containerCommand = new MacroCommand<MyContext>();

         _containerCommand.Add(_simpleCommand1);

         _macroCommand1 = new MacroCommand<MyContext>();
         _macroCommand1.Add(_simpleCommand2);

         _macroCommand2 = new MacroCommand<MyContext>();
         _macroCommand2.Add(_simpleCommand3, _simpleCommand4);

         _macroCommand1.Add(_macroCommand2);

         _containerCommand.Add(_macroCommand1);

         _containerCommand.Add(_simpleCommand5);
      }

      protected override void Because()
      {
         _result = sut.ExpandsAndKeep(_containerCommand);
      }

      [Test]
      public void should_return_the_sub_commands_defined_in_the_command()
      {
         _result.ShouldOnlyContainInOrder(_containerCommand, _simpleCommand1, _macroCommand1, _simpleCommand2, _macroCommand2, _simpleCommand3, _simpleCommand4, _simpleCommand5);
      }
   }
}