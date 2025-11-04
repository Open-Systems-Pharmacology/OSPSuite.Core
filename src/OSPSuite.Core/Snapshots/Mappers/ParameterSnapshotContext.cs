using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ParameterSnapshotContext : SnapshotContext
   {
      public IParameter Parameter { get; }

      public ParameterSnapshotContext(IParameter parameter, SnapshotContext baseContext) : base(baseContext)
      {
         Parameter = parameter;
      }
   }
}