namespace OSPSuite.Core.Commands.Core
{
    public interface IEmptyCommand : ICommand
    {
    }

    public class EmptyCommand<TExecutionContext> : Command, ICommand<TExecutionContext>, IEmptyCommand
    {
        public void Execute(TExecutionContext context)
        {
        }

        public void RestoreExecutionData(TExecutionContext context)
        {
        }
    }
}