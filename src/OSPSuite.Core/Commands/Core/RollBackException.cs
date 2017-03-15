using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands.Core
{
    public class RollBackException: OSPSuiteException
    {
       private const string _rollBackCommandExceptionMessage = "Cannot perform Rollback for irreversible action: '{0}'";

       public RollBackException(ICommand command)
            : base(_rollBackCommandExceptionMessage.FormatWith(command.Description))
        {
            
        }
    }
}