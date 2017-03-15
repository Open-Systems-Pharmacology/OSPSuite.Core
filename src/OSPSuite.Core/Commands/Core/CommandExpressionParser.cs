using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Commands.Core
{
    public interface ICommandExpressionParser
    {
        IEnumerable<ICommand> Simplify(IEnumerable<ICommand> commandToSimplify);
        IEnumerable<ICommand<TExecutionContext>> Simplify<TExecutionContext>(IEnumerable<ICommand<TExecutionContext>> commandToSimplify);
    }

    public class CommandExpressionParser : ICommandExpressionParser
    {
        private readonly ICommandsExpander _commandExpander;

        public CommandExpressionParser() : this(new CommandsExpander())
        {
        }

        public CommandExpressionParser(ICommandsExpander commandExpander)
        {
            _commandExpander = commandExpander;
        }

        private IList<ICommand> simplifyExtendedCommands(IList<ICommand> expandedCommandIdsToSimplify)
        {
            //Remove all adjacent pair of Ids|ReverseIds
            var simplifiedCommands = removeReverseIds(expandedCommandIdsToSimplify);
            if (simplifiedCommands.Count.Equals(expandedCommandIdsToSimplify.Count))
                return simplifiedCommands;

            return simplifyExtendedCommands(simplifiedCommands);
        }

        public IEnumerable<ICommand> Simplify(IEnumerable<ICommand> commandToSimplify)
        {
            return simplifyExtendedCommands(_commandExpander.Expands(commandToSimplify).ToList());
        }

        public IEnumerable<ICommand<TExecutionContext>> Simplify<TExecutionContext>(IEnumerable<ICommand<TExecutionContext>> commandToSimplify)
        {
            var simplifiedCommands = Simplify(commandToSimplify.Select(command => command as ICommand));
            return simplifiedCommands.Select(command => command as ICommand<TExecutionContext>);
        }

        private IList<ICommand> removeReverseIds(IList<ICommand> commandsToSimplify)
        {
            //nothing to simplify return the list
            if (commandsToSimplify.Count <= 1) return commandsToSimplify;

            var result = new List<ICommand>();
            for (int i = 0; i < commandsToSimplify.Count; i++)
            {
                if ((i < commandsToSimplify.Count - 1) && (commandsToSimplify[i + 1].IsInverseFor(commandsToSimplify[i])))
                    i++;
                else
                    result.Add(commandsToSimplify[i]);
            }

            return result;
        }
    }
}