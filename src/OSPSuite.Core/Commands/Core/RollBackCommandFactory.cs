using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
   public interface IRollBackCommandFactory
   {
      ICommand<TExecutionContext> CreateRollBackFor<TExecutionContext>(int state, TExecutionContext context, IEnumerable<ICommand<TExecutionContext>> commandsToReverse);
   }

   public class RollBackCommandFactory : IRollBackCommandFactory
   {
      private readonly ICommandExpressionParser _commandExpressionParser;

      public RollBackCommandFactory() : this(new CommandExpressionParser())
      {
      }

      public RollBackCommandFactory(ICommandExpressionParser commandExpressionParser)
      {
         _commandExpressionParser = commandExpressionParser;
      }

      public ICommand<TExecutionContext> CreateRollBackFor<TExecutionContext>(int state, TExecutionContext context, IEnumerable<ICommand<TExecutionContext>> commandsToReverse)
      {
         var simplifiedCommands = _commandExpressionParser.Simplify(commandsToReverse).ToList();

         if (simplifiedCommands.Count == 0)
            return new EmptyCommand<TExecutionContext>();

         var reversibleCommands = new List<IReversibleCommand<TExecutionContext>>();
         var allPossibleCommands = simplifiedCommands.Where(command => !command.IsAnImplementationOf<IInfoCommand>());
         foreach (var command in allPossibleCommands)
         {
            var reversibleCommand = command as IReversibleCommand<TExecutionContext>;
            if(reversibleCommand==null)
              throw new RollBackException(command);

            reversibleCommands.Add(reversibleCommand);
         }

         //reverse to execute last command executed first
         reversibleCommands.Reverse();
         return new RollBackCommand<TExecutionContext>(state, reversibleCommands);
      }
   }
}