using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
    public static class MacroCommandExtensions
    {
        public static IMacroCommand<ExecutionContext> Add<ExecutionContext>
            (this  IMacroCommand<ExecutionContext> macroCommand,params ICommand<ExecutionContext>[] commandsToAdd)
        {
            commandsToAdd.Each(macroCommand.Add);
            return macroCommand;
        }
    }
}