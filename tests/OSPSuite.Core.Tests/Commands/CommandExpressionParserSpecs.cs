using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_command_expression_simplify : ContextSpecification<ICommandExpressionParser>
   {
      protected IList<ICommand> _commandIdToSimplify;
      protected IEnumerable<ICommand> result;

      protected override void Context()
      {
         sut = new CommandExpressionParser();
      }

      protected override void Because()
      {
         result = sut.Simplify(_commandIdToSimplify);
      }
   }

   public class When_simplifying_a_command_expression_that_cannot_be_simplified : concern_for_command_expression_simplify
   {
      private ICommand _commandId2;
      private ICommand _commandId1;
      private ICommand _commandId3;

      [Observation]
      public void should_return_a_set_containing_all_original_commands_id_in_the_right_order()
      {
         result.ShouldOnlyContainInOrder(_commandId1, _commandId2, _commandId3);
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(-1,4)*C(-1,3)*C(-1,2)
         _commandId1 = new MySimpleCommand();
         _commandId2 = new MySimpleCommand();
         _commandId3 = new MySimpleCommand();
         _commandIdToSimplify = new List<ICommand>(new[] {_commandId1, _commandId2, _commandId3});
      }
   }

   public class When_simplifying_a_command_expression_that_can_be_simplified : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;

      [Observation]
      public void should_return_a_command_set_that_cannot_be_simplified_any_further()
      {
         result.ShouldOnlyContain(_command3);
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(1,1)*C(-1,1)*C(1,2)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand(_command1.Id.CreateInverseId());
         _command3 = new MyReversibleCommand();
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3});
      }
   }

   public class When_simplifying_a_command_expression_that_contains_only_one_commands_and_its_reverse : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;

      [Observation]
      public void should_not_simplify_too_many_items()
      {
         result.ShouldOnlyContain(_command3);
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(1,1)*C(-1,1)*C(1,1)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand(_command1.Id.CreateInverseId());
         _command3 = new MyReversibleCommand(_command2.Id.CreateInverseId());
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3});
      }
   }

   public class When_simplifying_a_command_expression_containing_inverses_but_that_cannot_be_simplified : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;

      [Observation]
      public void should_return_a_command_set_that_cannot_be_simplified_any_further()
      {
         result.ShouldOnlyContainInOrder(_command1, _command2, _command3);
      }

      protected override void Context()
      {
         base.Context();

         //C(1,1)*C(1,2)*C(-1,1)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         _command3 = new MyReversibleCommand(_command1.Id.CreateInverseId());
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3});
      }
   }

   public class When_simplifying_a_command_expression_with_two_level_of_simplification_that_can_be_simplified : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;
      private ICommand _command4;
      private ICommand _command5;
      private ICommand _command6;

      [Observation]
      public void should_return_a_command_set_that_cannot_be_simplified_any_further()
      {
         result.ShouldOnlyContainInOrder(_command1, _command6);
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(1,1)*C(-1,1)*C(1,2)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         _command3 = new MyReversibleCommand();
         _command4 = new MyReversibleCommand(_command3.Id.CreateInverseId());
         _command5 = new MyReversibleCommand(_command2.Id.CreateInverseId());
         _command6 = new MyReversibleCommand();
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3, _command4, _command5, _command6});
      }
   }

   public class When_simplifying_a_command_expression_with_the_simplification_occuring_at_the_end : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;
      private ICommand _command4;

      [Observation]
      public void should_return_a_command_set_that_cannot_be_simplified_any_further()
      {
         result.ShouldOnlyContainInOrder(_command1, _command2);
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(1,1)*C(-1,1)*C(1,2)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         _command3 = new MyReversibleCommand();
         _command4 = new MyReversibleCommand(_command3.Id.CreateInverseId());
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3, _command4});
      }
   }

   public class When_simplifying_a_command_expression_with_three_level_of_simplification_that_can_be_simplified : concern_for_command_expression_simplify
   {
      private ICommand _command1;
      private ICommand _command2;
      private ICommand _command3;
      private ICommand _command4;
      private ICommand _command5;
      private ICommand _command6;

      [Observation]
      public void should_return_a_command_set_that_cannot_be_simplified_any_further()
      {
         result.ShouldBeEmpty();
      }

      protected override void Context()
      {
         base.Context();

         //We want to simplify command with the ID's
         //C(1,1)*C(-1,1)*C(1,2)
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         _command3 = new MyReversibleCommand();
         _command4 = new MyReversibleCommand(_command3.Id.CreateInverseId());
         _command5 = new MyReversibleCommand(_command2.Id.CreateInverseId());
         _command6 = new MyReversibleCommand(_command1.Id.CreateInverseId());
         _commandIdToSimplify = new List<ICommand>(new[] {_command1, _command2, _command3, _command4, _command5, _command6});
      }
   }
}