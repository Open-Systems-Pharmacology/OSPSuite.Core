using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Snapshots
{
   public class SnapshotOutdatedException : OSPSuiteException
   {
      public SnapshotOutdatedException(string reason) : base($"{Error.SnapshotIsOutdated}\n\n{reason}")
      {
      }
   }
}