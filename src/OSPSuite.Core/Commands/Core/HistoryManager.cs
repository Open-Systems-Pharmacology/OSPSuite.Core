using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Events;

namespace OSPSuite.Core.Commands.Core
{
   public interface IHistoryManager
   {
      /// <summary>
      /// Add a command to the history (history item will be created according to current settings)
      /// </summary>
      void AddToHistory(ICommand commandToAddToHistory);

      /// <summary>
      /// Add an history item to the history
      /// </summary>
      /// <param name="historyItem"></param>
      void AddToHistory(IHistoryItem historyItem);

      /// <summary>
      /// Add a label command to the history
      /// </summary>
      /// <param name="labelCommand"></param>
      void AddLabel(ILabelCommand labelCommand);

      /// <summary>
      /// Returns the history items defined in the history
      /// </summary>
      IEnumerable<IHistoryItem> History { get; }

      /// <summary>
      /// Remove all history items defined in the history
      /// </summary>
      void Clear();

      /// <summary>
      /// Performs the roll back to the state given as parameter
      /// </summary>
      /// <param name="state">state to roll back to</param>
      void RollBackTo(int state);

      /// <summary>
      /// Event is raised whenever a command is added to the history
      /// </summary>
      event Action<IHistoryItem> CommandAdded;

      /// <summary>
      /// Performs an undo action (roll back to the state of the command executed before the last command)
      /// </summary>
      void Undo();

      /// <summary>
      /// Returns true if undo action can be performed
      /// </summary>
      bool CanUndo();
   }

   public interface IHistoryManager<TExecutionContext> : IHistoryManager
   {
      ICommand<TExecutionContext> CreateRollBackCommandTo(int state);
   }

   public class HistoryManager<TExecutionContext> : IHistoryManager<TExecutionContext>
   {
      private readonly TExecutionContext _executionContext;
      private readonly IEventPublisher _eventPublisher;
      private readonly IExceptionManager _exceptionManager;
      private readonly IList<IHistoryItem> _history;
      private readonly IHistoryItemFactory _historyItemFactory;
      private readonly IRollBackCommandFactory _rollBackCommandFactory;
      private int _currentState;

      private static int DEFAULT_STATE = 1;

      public event Action<IHistoryItem> CommandAdded = delegate { };

      public HistoryManager(TExecutionContext executionContext, IEventPublisher eventPublisher, IExceptionManager exceptionManager, IHistoryItemFactory historyItemFactory, IRollBackCommandFactory rollBackCommandFactory)
         : this(executionContext, eventPublisher, exceptionManager,historyItemFactory, rollBackCommandFactory,  new List<IHistoryItem>())
      {
      }


      internal HistoryManager(TExecutionContext executionContext, IEventPublisher eventPublisher, IExceptionManager exceptionManager, IHistoryItemFactory historyItemFactory, IRollBackCommandFactory rollBackCommandFactory, IList<IHistoryItem> history)
      {
         _executionContext = executionContext;
         _eventPublisher = eventPublisher;
         _exceptionManager = exceptionManager;
         _history = history;
         _historyItemFactory = historyItemFactory;
         _rollBackCommandFactory = rollBackCommandFactory;
         resetState();
      }

      public void AddToHistory(ICommand commandToAddToHistory)
      {
         if (!canAddCommandToHistory(commandToAddToHistory)) return;
         _currentState = nextAvailableState;
         addToHistory(commandToAddToHistory, _currentState);
      }

      public void AddToHistory(IHistoryItem historyItem)
      {
         addToHistory(historyItem, historyItem.State);
         _currentState = nextAvailableState;
      }

      public void AddLabel(ILabelCommand labelItemToAdd)
      {
         addToHistory(labelItemToAdd, _currentState);
      }

      private void addToHistory(IHistoryItem historyItem, int state)
      {
         historyItem.State = state;
         _history.Add(historyItem);
         CommandAdded(historyItem);
      }

      private void addToHistory(ICommand commandToAddToHistory, int state)
      {
         if (!canAddCommandToHistory(commandToAddToHistory)) return;
         addToHistory(_historyItemFactory.CreateFor(commandToAddToHistory), state);
      }

      public IEnumerable<IHistoryItem> History => _history;

      public void Clear()
      {
         _history.Clear();
         resetState();
      }

      private void resetState() => _currentState = DEFAULT_STATE;

      public void Undo()
      {
         if(!CanUndo()) return;
         
         var item = targetHistoryItemAfterUndo();
         if(item==null) return;

         RollBackTo(item.State);
      }

      public bool CanUndo()
      {
         //For undo, at least two items in history are required
         if (_history.Count <= 1)
            return false;

         return canUndo(hitoryItemToUndo());
      }

      private bool canUndo(IHistoryItem historyItem)
      {
         return historyItem != null && historyItem.Command.Loaded;
      }

      private IHistoryItem hitoryItemToUndo()
      {
         if (_history.Count ==0)
            return null;

         //-1 because we need the state to be undone
         return _history.ElementAt(_history.Count - 1);
      }

      private IHistoryItem targetHistoryItemAfterUndo()
      {
         if (_history.Count <= 1)
            return null;

         //-2 because we need the state of the one before last command 
         return _history.ElementAt(_history.Count - 2);
      }

      public void RollBackTo(int state)
      {
         try
         {
            publishEvent(new RollBackStartedEvent());
            var rollBackCommand = CreateRollBackCommandTo(state);
            if (rollBackCommand.IsEmpty()) return;
            _currentState = state;
            rollBackCommand.Execute(_executionContext);
            addToHistory(rollBackCommand, _currentState);
         }
         catch (Exception e)
         {
            _exceptionManager.LogException(e);
         }
         finally
         {
            publishEvent(new RollBackFinishedEvent());
         }
      }

      private void publishEvent<T>(T eventToPublish)
      {
         _eventPublisher.PublishEvent(eventToPublish);
      }

      public ICommand<TExecutionContext> CreateRollBackCommandTo(int state)
      {
         return _rollBackCommandFactory.CreateRollBackFor(state, _executionContext, getCommandsToReverseFor(state));
      }

      private IEnumerable<ICommand<TExecutionContext>> getCommandsToReverseFor(int state)
      {
         var commandsToReverse = new List<ICommand<TExecutionContext>>();
         if (state >= nextAvailableState) return commandsToReverse;

         for (int i = _history.Count - 1; i >= 0; i--)
         {
            var history = _history[i];

            if (history.State == state)
               break;

            if (history.Command.IsAnImplementationOf<IInfoCommand>())
               continue;

            var commandToAdd = history.Command as IReversibleCommand<TExecutionContext>;
            if (commandToAdd == null || !commandToAdd.Loaded)
               throw new RollBackException(history.Command);

            commandsToReverse.Add(commandToAdd);
         }
         commandsToReverse.Reverse();
         return commandsToReverse;
      }

      private bool canAddCommandToHistory(ICommand commandToAddToHistory)
      {
         if (commandToAddToHistory == null) 
            return false;

         return !commandToAddToHistory.IsEmpty() && !commandToAddToHistory.IsEmptyMacro();
      }

      private int nextAvailableState
      {
         get
         {
            var maxState = _history.Count == 0 ? 0 : _history.Max(x => x.State);
            return maxState + 1;
         }
      }
   }
}