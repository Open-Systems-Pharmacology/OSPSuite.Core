using System.Collections.Generic;

namespace OSPSuite.Core.Commands.Core
{
   public interface ICommandsExpander
   {
      /// <summary>
      /// Expands the macro commands defined in the list of commands. Do not keep the macro commands
      /// </summary>
      IEnumerable<ICommand> Expands(IEnumerable<ICommand> listOfCommands);

      /// <summary>
      /// Expands the macro commands defined in the command. Do not keep the macro command
      /// </summary>
      IEnumerable<ICommand> Expands(ICommand command);

      /// <summary>
      /// Expands the macro commands defined in the command and keep the macro command in the list.
      /// </summary>
      IEnumerable<ICommand> ExpandsAndKeep(ICommand command);
   }

   public class CommandsExpander : ICommandsExpander
   {
      public IEnumerable<ICommand> Expands(IEnumerable<ICommand> listOfCommands)
      {
         return expands(listOfCommands,false);
      }

      public IEnumerable<ICommand> Expands(ICommand command)
      {
         return expands(new List<ICommand> { command }, false);
      }

      public IEnumerable<ICommand> ExpandsAndKeep(ICommand command)
      {
         return expands(new List<ICommand> {command}, true);
      }

      private IEnumerable<ICommand> expands(IEnumerable<ICommand> listOfCommands, bool keepMacroCommand)
      {
         var commandIdList = new List<ICommand>();
         foreach (var command in listOfCommands)
         {
            var macroCommand = command as IMacroCommand;
            if (macroCommand == null)
               commandIdList.Add(command);
            else
            {
               if (keepMacroCommand)
                  commandIdList.Add(command);

               commandIdList.AddRange(expands(macroCommand.All(),keepMacroCommand));
            }
         }
         return commandIdList;
      } 
   }
}