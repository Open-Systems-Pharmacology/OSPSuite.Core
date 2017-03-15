using System;
using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Commands.Core
{
   public class NullHistoryManager : IHistoryManager
   {
      public void AddToHistory(ICommand commandToAddToHistory)
      {
      }

      public void AddToHistory(IHistoryItem historyItem)
      {
      }

      public void AddLabel(ILabelCommand labelCommand)
      {
      }

      public IEnumerable<IHistoryItem> History
      {
         get { return new List<IHistoryItem>(); }
      }

      public void Clear()
      {
      }

      public void RollBackTo(int state)
      {
      }

      public event Action<IHistoryItem> CommandAdded = delegate { };

      public void Undo()
      {
         
      }

      public bool CanUndo()
      {
         return false;
      }

      public IEventPublisher EventPublisher { get; set; }
      public IExceptionManager ExceptionManager { get; set; }
   }
}