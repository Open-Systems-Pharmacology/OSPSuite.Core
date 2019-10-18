using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public class When_adding_commands_to_a_macro_command : StaticContextSpecification
   {
      private IMacroCommand<MyContext> _macroCommand;
      private ICommand<MyContext> _commandToAdd1;
      private ICommand<MyContext> _commandToAdd2;
      private ICommand<MyContext> _commandToAdd3;

      [Observation]
      public void the_macro_command_should_contain_the_added_commands()
      {
         _macroCommand.All().ShouldOnlyContainInOrder(_commandToAdd1, _commandToAdd2, _commandToAdd3);
      }

      protected override void Because()
      {
         _commandToAdd1 = A.Fake<ICommand<MyContext>>();
         _commandToAdd2 = A.Fake<ICommand<MyContext>>();
         _commandToAdd3 = A.Fake<ICommand<MyContext>>();
         _macroCommand.Add(_commandToAdd1, _commandToAdd2, _commandToAdd3);
      }

      protected override void Context()
      {
         _macroCommand = new MacroCommand<MyContext>();
      }
   }
}