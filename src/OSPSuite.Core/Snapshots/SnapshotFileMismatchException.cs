using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Snapshots
{
   public class SnapshotFileMismatchException : OSPSuiteException
   {
      public SnapshotFileMismatchException(string desiredType, IEnumerable<string> reasons) : base($"{Error.SnapshotFileMismatch(desiredType)}\n\n{reasons.ToString("\n")}")
      {
      }
   }
}