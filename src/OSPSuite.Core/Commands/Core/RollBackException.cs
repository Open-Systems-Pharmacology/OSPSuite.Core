using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Commands.Core
{
   public class RollBackException : OSPSuiteException
   {
      public RollBackException(ICommand command)
         : base($"Cannot perform Rollback for irreversible action: '{command.Description}'")
      {
      }
   }
}