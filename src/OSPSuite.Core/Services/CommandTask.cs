using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Services
{
   public interface ICommandTask
   {
      void ResetChanges<TContext>(ICommand<TContext> commandToReset, TContext context);
   }

   public class CommandTask : ICommandTask
   {
      public void ResetChanges<TContext>(ICommand<TContext> commandToReset, TContext context)
      {
         var reversibleCommand = commandToReset as IReversibleCommand<TContext>;
         if (reversibleCommand == null) return;

         reversibleCommand.RestoreExecutionData(context);
         var inverseCommand = reversibleCommand.InverseCommand(context);
         inverseCommand.Run(context);
      }
   }
}