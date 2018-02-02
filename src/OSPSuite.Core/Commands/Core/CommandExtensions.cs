using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
   public static class CommandExtensions
   {
      public static T Run<T, ExecutionContext>(this T commandToExecute, ExecutionContext context) where T : ICommand<ExecutionContext>
      {
         commandToExecute.Execute(context);
         return commandToExecute;
      }

      public static T AsInverseFor<T>(this T inverseCommand, ICommand originalCommand) where T : ICommand
      {
         inverseCommand.Id = originalCommand.Id.CreateInverseId();
         inverseCommand.ObjectType = originalCommand.ObjectType;
         return inverseCommand;
      }

      public static bool IsEmpty(this ICommand command)
      {
         return command == null || command.IsAnImplementationOf<IEmptyCommand>();
      }

      public static bool IsEmptyMacro(this ICommand command)
      {
         var macroCommand = command as IMacroCommand;
         return macroCommand != null && macroCommand.IsEmtpy;
      }

      public static T AsHidden<T>(this T command) where T : ICommand
      {
         command.Visible = false;
         return command;
      }
   }
}