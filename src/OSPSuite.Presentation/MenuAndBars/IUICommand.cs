using System;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IUICommand
   {
      void Execute();
   }
   public interface IUICommandAsync
   {
      Task ExecuteAsync();
   }

   public class NotImplementedCommandAsync : IUICommandAsync
   {
      public Task ExecuteAsync()
      {
         throw new NotSupportedException("Action is not implemented yet");
      }
   }

   public class NotImplementedCommand : IUICommand
   {
      public void Execute()
      {
         throw new NotSupportedException("Action is not implemented yet");
      }
   }

   public class EmptyCommand : IUICommand
   {
      public void Execute()
      {
      }
   }

   public class ExecuteActionUICommand : IUICommand
   {
      private readonly Action _actionToExecute;

      public ExecuteActionUICommand(Action actionToExecute)
      {
         _actionToExecute = actionToExecute;
      }

      public void Execute()
      {
         _actionToExecute();
      }
   }

   public class ExecuteActionUICommandAsync : IUICommandAsync
   {
      private readonly Func<Task> _asyncActionToExecute;

      public ExecuteActionUICommandAsync(Func<Task> actionToExecute)
      {
         _asyncActionToExecute = actionToExecute;
      }

      public async Task ExecuteAsync()
      {
         await _asyncActionToExecute();
      }
   }
}