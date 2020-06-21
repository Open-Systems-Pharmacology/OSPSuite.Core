using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_HistoryManager : ContextSpecification<IHistoryManager<MyContext>>
   {
      protected IList<IHistoryItem> _historyItems;
      protected IHistoryItemFactory _historyItemFactory;
      protected MyContext _context;
      protected IEventPublisher _eventPublisher;
      protected IExceptionManager _exceptionManager;

      protected override void Context()
      {
         _context = new MyContext();
         _historyItems = new List<IHistoryItem>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _exceptionManager = A.Fake<IExceptionManager>();
         sut = new HistoryManager<MyContext>(_context, _eventPublisher, _exceptionManager, new HistoryItemFactory(), new RollBackCommandFactory(), _historyItems);
      }
   }

   public class When_adding_a_command_to_the_history : concern_for_HistoryManager
   {
      private ICommand<MyContext> _command;
      private ICommand<MyContext> _anotherCommnand;

      protected override void Context()
      {
         base.Context();
         _command = new MyReversibleCommand();
         _anotherCommnand = new MyReversibleCommand();
      }

      protected override void Because()
      {
         sut.AddToHistory(_command);
         sut.AddToHistory(_anotherCommnand);
      }

      [Observation]
      public void should_add_a_new_entry_to_the_history_for_the_given_commands()
      {
         sut.History.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_adding_an_empty_or_null_command_to_the_history : concern_for_HistoryManager
   {
      private ICommand<MyContext> _command;
      private ICommand<MyContext> _validCommand;
      private int _stateBeforeAddingInvalidCommands;
      private ICommand<MyContext> _anotherCommand;

      protected override void Context()
      {
         base.Context();
         _validCommand = A.Fake<ICommand<MyContext>>();
         _command = new EmptyCommand<MyContext>();
         _anotherCommand = null;
         _stateBeforeAddingInvalidCommands = 0;
      }

      protected override void Because()
      {
         sut.AddToHistory(_command);
         sut.AddToHistory(_anotherCommand);
      }

      [Observation]
      public void should_not_change_the_history()
      {
         sut.History.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_not_have_change_the_current_state()
      {
         //add a valid command. It states should be the one before adding the invalid commands +1
         sut.AddToHistory(_validCommand);
         sut.History.Last().State.ShouldBeEqualTo(_stateBeforeAddingInvalidCommands + 1);
      }
   }

   public class When_adding_an_label : concern_for_HistoryManager
   {
      private ILabelCommand _label;

      protected override void Context()
      {
         base.Context();
         _label = A.Fake<ILabelCommand>();
      }

      protected override void Because()
      {
         sut.AddLabel(_label);
      }

      [Observation]
      public void should_add_a_new_entry_to_the_history()
      {
         sut.History.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_adding_an_empty_macro_command_to_the_history : concern_for_HistoryManager
   {
      private ICommand<MyContext> _command;

      protected override void Context()
      {
         base.Context();
         _command = new MacroCommand<MyContext>();
      }

      protected override void Because()
      {
         sut.AddToHistory(_command);
      }

      [Observation]
      public void should_not_change_the_history()
      {
         sut.History.Count().ShouldBeEqualTo(0);
      }
   }

   public abstract class concern_for_HistoryManagerRollBack : concern_for_HistoryManager
   {
      protected IReversibleCommand<MyContext> _command1;
      protected IReversibleCommand<MyContext> _command2;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
         sut.AddLabel(A.Fake<ILabelCommand>());
         sut.AddLabel(A.Fake<ILabelCommand>());
      }
   }

   public class When_performing_a_roll_back_to_a_valid_state : concern_for_HistoryManagerRollBack
   {
      private IMacroCommand<MyContext> _result;

      protected override void Because()
      {
         sut.RollBackTo(0);
         _result = sut.History.Last().Command as IRollBackCommand<MyContext>;
      }

      [Observation]
      public void should_create_a_new_roll_back_command_containing_the_reverse_comamnds_and_add_it_at_the_end_of_the_history()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_ignore_any_label_commands()
      {
         _result.All().Count().ShouldBeEqualTo(2);
      }
   }

   public class When_adding_a_command_to_the_history_after_a_rollback_was_performed : concern_for_HistoryManagerRollBack
   {
      private ICommand _commandToAddHistory;

      protected override void Context()
      {
         base.Context();
         _commandToAddHistory = A.Fake<ICommand>();
         sut.RollBackTo(0);
      }

      protected override void Because()
      {
         sut.AddToHistory(_commandToAddHistory);
      }

      [Observation]
      public void the_state_of_that_command_should_be_a_new_state()
      {
         sut.History.Last().State.ShouldBeEqualTo(3);
      }
   }

   public class When_performing_a_roll_back_to_a_state_for_which_not_reversible_commands_need_to_be_undone : concern_for_HistoryManagerRollBack
   {
      private Exception _exception;

      protected override void Context()
      {
         base.Context();
         sut.AddToHistory(new MyReversibleCommand {Loaded = false});
         A.CallTo(() => _exceptionManager.LogException(A<Exception>.Ignored)).Invokes(
            x => { _exception = x.Arguments.Get<Exception>(0); });
      }

      protected override void Because()
      {
         sut.RollBackTo(0);
      }

      [Observation]
      public void should_throw_a_roll_back_exception()
      {
         _exception.ShouldBeAnInstanceOf<RollBackException>();
      }
   }

   public class When_performing_a_roll_back_to_a_state_for_which_unloaded_commands_need_to_be_undone : concern_for_HistoryManagerRollBack
   {
      private Exception _exception;

      protected override void Context()
      {
         base.Context();
         sut.AddToHistory(new MySimpleCommand());
         A.CallTo(() => _exceptionManager.LogException(A<Exception>.Ignored)).Invokes(
            x => { _exception = x.Arguments.Get<Exception>(0); });
      }

      protected override void Because()
      {
         sut.RollBackTo(0);
      }

      [Observation]
      public void should_throw_a_roll_back_exception()
      {
         _exception.ShouldBeAnInstanceOf<RollBackException>();
      }
   }

   public class When_the_roll_back_actions_throws_an_exception_and_the_exception_manager_was_defined : concern_for_HistoryManagerRollBack
   {
      protected override void Context()
      {
         base.Context();
         sut.AddToHistory(new MySimpleCommand());
      }

      protected override void Because()
      {
         sut.RollBackTo(0);
      }

      [Observation]
      public void should_log_the_exception()
      {
         A.CallTo(() => _exceptionManager.LogException(A<Exception>.Ignored)).MustHaveHappened();
      }
   }

   public class When_creating_a_roll_back_command_containing_commands_that_simplify_themselves_completly : concern_for_HistoryManagerRollBack
   {
      private ICommand<MyContext> _result;

      protected override void Because()
      {
         _result = sut.CreateRollBackCommandTo(0);
      }

      protected override void Context()
      {
         base.Context();
         sut.AddToHistory(_command2.InverseCommand(_context));
         sut.AddToHistory(_command1.InverseCommand(_context));
      }

      [Observation]
      public void should_return_an_empty_command()
      {
         _result.ShouldBeAnInstanceOf<EmptyCommand<MyContext>>();
      }
   }

   public class When_rolling_back_to_a_state_for_which_the_roll_back_command_is_empty : concern_for_HistoryManagerRollBack
   {
      private int _orignialCount;

      protected override void Because()
      {
         sut.RollBackTo(0);
      }

      protected override void Context()
      {
         base.Context();
         sut.AddToHistory(_command2.InverseCommand(_context));
         sut.AddToHistory(_command1.InverseCommand(_context));
         _orignialCount = sut.History.Count();
      }

      [Observation]
      public void should_not_change_the_history()
      {
         _orignialCount.ShouldBeEqualTo(sut.History.Count());
      }
   }

   public class When_rolling_back_to_a_state_that_does_not_exist : concern_for_HistoryManagerRollBack
   {
      private int _orignialCount;

      protected override void Context()
      {
         base.Context();
         _orignialCount = sut.History.Count();
      }

      protected override void Because()
      {
         sut.RollBackTo(50);
      }

      [Observation]
      public void should_not_change_the_history()
      {
         _orignialCount.ShouldBeEqualTo(sut.History.Count());
      }
   }

   public class When_rolling_back_to_a_state_that_already_contains_roll_back : concern_for_HistoryManagerRollBack
   {
      private IReversibleCommand<MyContext> _command3;
      //1  2  1   3  1  
      //C1 C2 C-2 C3 C-3  roll back to 3
      protected override void Context()
      {
         base.Context();
         _command3 = new MyReversibleCommand();
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
         sut.RollBackTo(1);
         sut.AddToHistory(_command3);
         sut.RollBackTo(1);
      }

      protected override void Because()
      {
         sut.RollBackTo(3);
      }

      [Observation]
      public void should_be_able_to_roll_back_to_the_state()
      {
         sut.History.Last().State.ShouldBeEqualTo(3);
      }
   }

   public class When_rolling_back_to_a_state_that_results_in_an_empty_command_being_executed_and_adding_a_label_command : concern_for_HistoryManagerRollBack
   {
      //Inf01 - Inf02
      protected override void Context()
      {
         base.Context();
         sut = new HistoryManager<MyContext>(_context, _eventPublisher, _exceptionManager, new HistoryItemFactory(), new RollBackCommandFactory());
         sut.AddToHistory(new InfoCommand());
         sut.AddToHistory(new InfoCommand());
         sut.RollBackTo(1);
      }

      protected override void Because()
      {
         sut.AddLabel(new LabelCommand());
      }

      [Observation]
      public void should_not_have_reset_the_current_state_and_add_the_new_command_at_the_expected_state()
      {
         sut.History.Last().State.ShouldBeEqualTo(2);
      }
   }

   public class When_rolling_back_to_a_state_that_contains_a_macro_command : concern_for_HistoryManagerRollBack
   {
      private IMacroCommand<MyContext> _command3;
      private IReversibleCommand<MyContext> _command31;
      private IReversibleCommand<MyContext> _command32;
      private IReversibleCommand<MyContext> _inverseCommand32;
      private IReversibleCommand<MyContext> _inverseCommand31;
      private IReversibleCommand<MyContext> _inverseCommand2;
      private IReversibleCommand<MyContext> _command0;
      //1    2    3         4 
      //  C1   C2   C31 C32    roll back to 1
      protected override void Context()
      {
         _context = new MyContext();
         _historyItems = new List<IHistoryItem>();
         _command0 = new MyReversibleCommand {InternalId = "_command0"};
         _command1 = new MyReversibleCommand {InternalId = "_command1"};
         _command2 = new MyReversibleCommand {InternalId = "_command2"};
         _command31 = new MyReversibleCommand {InternalId = "_command31"};
         _command32 = new MyReversibleCommand {InternalId = "_command32"};
         _inverseCommand32 = _command32.InverseCommand(_context);
         _inverseCommand31 = _command31.InverseCommand(_context);
         _inverseCommand2 = _command2.InverseCommand(_context);
         _exceptionManager = A.Fake<IExceptionManager>();
         _eventPublisher = A.Fake<IEventPublisher>();

         sut = new HistoryManager<MyContext>(_context, _eventPublisher, _exceptionManager, new HistoryItemFactory(), new RollBackCommandFactory(), _historyItems);

         sut.AddToHistory(_command0);
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
         _command3 = new MacroCommand<MyContext>();
         _command3.Add(_command31);
         _command3.Add(_command32);
         sut.AddToHistory(_command3);
      }

      protected override void Because()
      {
         sut.RollBackTo(2);
      }

      [Observation]
      public void should_execute_the_command_in_the_accurate_order()
      {
         var command = _historyItems.Last().Command as MacroCommand<MyContext>;
         command.ShouldNotBeNull();

         var subCommand = command.All().ToList();
         subCommand[0].Id.InverseId.ShouldBeEqualTo(_inverseCommand32.Id.InverseId);
         subCommand[1].Id.InverseId.ShouldBeEqualTo(_inverseCommand31.Id.InverseId);
         subCommand[2].Id.InverseId.ShouldBeEqualTo(_inverseCommand2.Id.InverseId);
      }
   }

   public class When_execution_an_undo : concern_for_HistoryManagerRollBack
   {
      protected override void Because()
      {
         sut.Undo();
      }

      [Observation]
      public void should_perform_a_roll_back_to_the_state_of_the_second_command_in_the_list()
      {
         var command = _historyItems.Last();
         command.State.ShouldBeEqualTo(2);
      }
   }

   public class When_asked_if_an_undoable_state_can_be_undone_ : concern_for_HistoryManager
   {
      protected IReversibleCommand<MyContext> _command1;
      protected IReversibleCommand<MyContext> _command2;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
         _command2.Loaded = true;
      }

      [Observation]
      public void should_return_true()
      {
         sut.CanUndo().ShouldBeTrue();
      }
   }

   public class When_asked_if_an_not_undoable_state_can_be_undone_ : concern_for_HistoryManager
   {
      protected IReversibleCommand<MyContext> _command1;
      protected IReversibleCommand<MyContext> _command2;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command2 = new MyReversibleCommand();
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
         _command2.Loaded = false;
      }

      [Observation]
      public void should_return_false()
      {
         sut.CanUndo().ShouldBeFalse();
      }
   }

   public class When_undoing_an_history_with_only_one_command : concern_for_HistoryManager
   {
      protected IReversibleCommand<MyContext> _command1;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand();
         _command1.Loaded = true;
         sut.AddToHistory(_command1);
      }

      [Observation]
      public void should_not_crash_when_doing_the_undo()
      {
         sut.Undo();
      }
   }

   public class When_clearing_the_history : concern_for_HistoryManager
   {
      protected IReversibleCommand<MyContext> _command1;
      protected IReversibleCommand<MyContext> _command2;
      protected ILabelCommand _labelCommand;

      protected override void Context()
      {
         base.Context();
         _command1 = new MyReversibleCommand {Loaded = true};
         _command2 = new MyReversibleCommand {Loaded = true};
         _labelCommand=new LabelCommand();
         sut.AddToHistory(_command1);
         sut.AddToHistory(_command2);
      }

      protected override void Because()
      {
          sut.Clear();
      }

      [Observation]
      public void should_remove_all_history_entries_from_the_list()
      {
         sut.History.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void adding_a_label_should_have_set_the_state_to_one()
      {
         sut.AddLabel(_labelCommand);
         sut.History.ElementAt(0).State.ShouldBeEqualTo(1);
      }
   }
}