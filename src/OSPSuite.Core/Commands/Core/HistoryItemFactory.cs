using System;
using System.Linq;
using OSPSuite.Utility;

namespace OSPSuite.Core.Commands.Core
{
    public interface IHistoryItemFactory
    {
        IHistoryItem CreateFor(ICommand command);
    }

    public class HistoryItemFactory : IHistoryItemFactory
    {
        public IHistoryItem CreateFor(ICommand command)
        {
            return new HistoryItem(EnvironmentHelper.UserName(), 
                                   SystemTime.Now(), 
                                   CommandToRegisterFrom(command))
                      {
                         Id= Guid.NewGuid().ToString()
                      };
        }

        private ICommand CommandToRegisterFrom(ICommand command)
        {
            var macroCommand = command as IMacroCommand;
            if (macroCommand == null) return command;
            if (macroCommand.Count != 1) return macroCommand;
            return macroCommand.All().ElementAt(0);
        }
    }
}