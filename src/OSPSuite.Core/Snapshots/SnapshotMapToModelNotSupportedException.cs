using System;
using OSPSuite.Assets;

namespace OSPSuite.Core.Snapshots
{
   public class SnapshotMapToModelNotSupportedException<TModel, TContext> : NotSupportedException
   {
      public SnapshotMapToModelNotSupportedException() : base(Error.MapToModelNotSupportedWithoutContext(typeof(TModel).Name, typeof(TContext).Name))
      {
      }
   }

   public class ModelMapToSnapshotNotSupportedException<TSnapshot, TContext> : NotSupportedException
   {
      public ModelMapToSnapshotNotSupportedException() : base(Error.MapToSnapshotNotSupportedWithoutContext(typeof(TSnapshot).Name, typeof(TContext).Name))
      {
      }
   }
}