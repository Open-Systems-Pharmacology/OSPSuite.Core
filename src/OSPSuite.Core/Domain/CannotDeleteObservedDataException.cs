using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public class CannotDeleteObservedDataException : OSPSuiteException
   {
      public CannotDeleteObservedDataException(string observedDataName, IReadOnlyList<string> usersOfObservedData)
      {
         Message = Error.CannotDeleteBuildingBlockUsedBy(ObjectTypes.ObservedData, observedDataName, usersOfObservedData);
      }

      public override string Message { get; }
   }
}