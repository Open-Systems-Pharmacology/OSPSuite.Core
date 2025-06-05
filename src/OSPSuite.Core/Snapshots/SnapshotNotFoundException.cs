using System;
using OSPSuite.Assets;

namespace OSPSuite.Core.Snapshots
{
   public class SnapshotNotFoundException : Exception
   {
      public SnapshotNotFoundException(Type modelType) : base(Error.SnapshotNotFoundFor(modelType.FullName))
      {
      }
   }
}