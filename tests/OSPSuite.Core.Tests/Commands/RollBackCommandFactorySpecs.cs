using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_RollBackCommandFactory : ContextSpecification<IRollBackCommandFactory>
   {
      protected IList<ICommand<MyContext>> _inverseCommandList;
      protected ICommandExpressionParser _commandExpressionParser;
      protected ICommand<MyContext> _result;
      protected int _state;
      protected MyContext _context;

      protected override void Context()
      {
         _commandExpressionParser = A.Fake<ICommandExpressionParser>();
         _inverseCommandList = new List<ICommand<MyContext>>();
         _context = new MyContext();
         sut = new RollBackCommandFactory(_commandExpressionParser);
      }
   }

   public class When_creating_a_roll_back_command_for_an_empty_list_of_simplified_commands : concern_for_RollBackCommandFactory
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _commandExpressionParser.Simplify(_inverseCommandList)).Returns(_inverseCommandList);
      }

      protected override void Because()
      {
         _state = 2;
         _result = sut.CreateRollBackFor(_state, _context, _inverseCommandList);
      }

      [Observation]
      public void should_return_an_empty_command()
      {
         _result.ShouldBeAnInstanceOf<EmptyCommand<MyContext>>();
      }
   }

   public class When_creating_a_roll_back_command_for_a_list_of_simplified_commands_containing_two_or_more_sub_commands : concern_for_RollBackCommandFactory
   {
      private ICommand<MyContext> _command1;
      private ICommand<MyContext> _command2;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         A.CallTo(() => _commandExpressionParser.Simplify(_inverseCommandList)).Returns(new[] {_command1, _command2});
      }

      protected override void Because()
      {
         _state = 2;
         _result = sut.CreateRollBackFor(_state, _context, _inverseCommandList);
      }

      [Observation]
      public void should_return_a_roll_back_command()
      {
         _result.ShouldBeAnInstanceOf<IRollBackCommand<MyContext>>();
      }
   }

   public class When_creating_a_roll_back_command_for_a_list_of_simplified_commands_containg_some_info_command : concern_for_RollBackCommandFactory
   {
      private ICommand<MyContext> _command1;
      private ICommand<MyContext> _command2;
      private ICommand<MyContext> _command3;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         _command3 = new MyInfoCommand();
         _inverseCommandList.Add(_command1);
         _inverseCommandList.Add(_command2);
         _inverseCommandList.Add(_command3);
         A.CallTo(() => _commandExpressionParser.Simplify(_inverseCommandList)).Returns(new[] {_command1, _command2, _command3});
      }

      protected override void Because()
      {
         _state = 2;
         _result = sut.CreateRollBackFor(_state, _context, _inverseCommandList);
      }

      [Observation]
      public void should_return_a_roll_back_command_ignoring_the_info_command()
      {
         _result.Execute(_context);
         _result.ShouldBeAnInstanceOf<IRollBackCommand<MyContext>>();
         _result.DowncastTo<IMacroCommand>().Count.ShouldBeEqualTo(2);
      }

      private class MyInfoCommand : InfoCommand, ICommand<MyContext>
      {
         public void Execute(MyContext context)
         {
            //nothing to do
         }
      }
   }

   public class When_creating_a_roll_back_command_for_a_macro_command_containg_an_irreversible_command : concern_for_RollBackCommandFactory
   {
      protected override void Context()
      {
         base.Context();
         var macroCommand = new MacroCommand<MyContext>();
         A.CallTo(() => _commandExpressionParser.Simplify(_inverseCommandList)).Returns(new ICommand<MyContext>[] {new MyReversibleCommand(), new MySimpleCommand()});
         _inverseCommandList.Add(macroCommand);
      }

      [Observation]
      public void should_throw_a_rollback_command_exception()
      {
         The.Action(() => sut.CreateRollBackFor(2, _context, _inverseCommandList)).ShouldThrowAn<RollBackException>();
      }
   }
}