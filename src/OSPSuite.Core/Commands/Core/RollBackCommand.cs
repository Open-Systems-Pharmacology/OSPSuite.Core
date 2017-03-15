using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
    internal interface IRollBackCommand<in TExecutionContext> : IMacroCommand<TExecutionContext>
    {
    }

    internal class RollBackCommand<TExecutionContext> : MacroCommand<TExecutionContext>, IRollBackCommand<TExecutionContext>
    {
        private readonly IList<IReversibleCommand<TExecutionContext>> _reversibleCommands;

        public RollBackCommand(int state, IEnumerable<IReversibleCommand<TExecutionContext>> reversibleCommands)
            : this(state, reversibleCommands.ToList())
        {
        }

        public RollBackCommand(int state, IList<IReversibleCommand<TExecutionContext>> reversibleCommands)
        {
            _reversibleCommands = reversibleCommands;
            CommandType = "Rollback";
            Description = string.Format("Rollback to state {0}", state);
        }

        public override void Execute(TExecutionContext context)
        {
            _subCommands.Clear();
            _reversibleCommands.Each(command =>
                                              {
                                                  command.RestoreExecutionData(context);
                                                  var inverseCommand = command.InverseCommand(context);
                                                  inverseCommand.Execute(context);
                                                  _subCommands.Add(inverseCommand);
                                              });
        }
    }
}