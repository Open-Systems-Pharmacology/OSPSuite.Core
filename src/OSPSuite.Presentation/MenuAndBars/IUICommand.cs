using System;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IUICommand
   {
      void Execute();
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
}