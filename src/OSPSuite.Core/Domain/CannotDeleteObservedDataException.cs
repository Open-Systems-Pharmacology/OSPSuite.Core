using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class CannotDeleteObservedDataException : OSPSuiteException
   {
      public CannotDeleteObservedDataException(IEnumerable<string> observedDataErrors) : base(observedDataErrors.ToString("\n\n"))
      {
      }

      public CannotDeleteObservedDataException(string observedDataName, IReadOnlyList<string> usersOfObservedData) : base(Error.CannotDeleteBuildingBlockUsedBy(ObjectTypes.ObservedData, observedDataName, usersOfObservedData))
      {
         
      }
   }
}